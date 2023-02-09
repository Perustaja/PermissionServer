# PermissionServer
[Check out the demo!](https://github.com/Perustaja/PermissionServerDemo)
PermissionServer is a library that allows applications to make authorization decisions without storing the authorization information (roles, permissions, etc.) in a JWT. PermissionServer makes use of finely-grained permissions as opposed to ASP.NET claims, roles, or similar.

## What PermissionServer does
- Adds local and/or remote authorization to the application with attributes for controllers and controller actions.
- Manages developer-designed permissions, including synchronizing them to a database if desired.
- Designed for use with either a multi-tenant system, or a single-tenant one with different packages for either.

## What PermissionServer doesn't do
- Add in multi-tenancy itself, you must make your own system for this.
- Provide a default way to evaluate authorization. You yourself must implement the interface.
Basically, if you want to see a full working example, see the demo. The demo includes custom tenant roles and multi-tenancy. I am mulling over adding this but this will take time and may not be added, as the amount of generics may be too much.

## Constraints
PermissionServer, like any form of authorization library, is opinionated. It may not be what is best for your project. It may be close, but you may need to expand upon it. 
- You must use the suggested method of defining permissions and permission categories. You must use enums, and not some form of ASP.NET claims.
- You must make authorization decisions solely based on a user id, a tenant id (unless you're using the single-tenant package), and an optional list of permissions.
- Only one protected resource is supported right now, but support for multiple will be added shortly.

# Usage
Attributes are used to protected endpoints, which may be local or remote. A list of permissions required for the endpoint are used as arguments for the attribute. In the multi-tenant base package, no arguments means that the user should solely be verified as having access to the tenant.

Local authorization means that the information does not require a network call, for instance if your identity provider allows tenants to customize their company, you may want to ensure that the user has the right to do so within that tenant. 
```
[HttpGet("organizations/{tenantId}/roles")]    
[LocalAuthorize<MyPermission>]    
public async Task<IActionResult> GetTenantRoles(...) { ... }
```

or similarly with permissions required
```
[HttpPost("organizations/{tenantId}/roles")]
[LocalAuthorize<MyPermission>(PermissionEnum.RolesCreate)]
public async Task<IActionResult> CreateTenantRole(...) { ... }
```

Remote authorization means that the identity provider will be called, and is used by e.g. an API
```
[HttpPost]
[Route("{tenantId}/aircraft")]
[RemoteAuthorize<MyPermission>(PermissionEnum.AircraftCreate)]
public async Task<IActionResult> Post(...) { ... }
```
The generic constraint can be removed by [following these instructions](#non-generic-authorize-attributes).
# Setup + Configuration
Add the package with ```dotnet add package PermissionServer``` or alternately ```dotnet add package PermissionServer.Singletenant```. Both packages are very similar with a few difference explained [here](#multi-tenant-package-vs-single-tenant-package). You must also be using C# 11.0 if you're using the default provided attributes, if you're using net6.0 this can be achieved by adding ```<LangVersion>preview</LangVersion>``` to your .csproj.

## Enums
Define your permission and permission category enums. These have optional attributes which contain data about your attribute. Examples are below.
```
public enum MyPermission : byte
{
    [PermissionData<MyPermissionCategory>(MyPermissionCategory.Aircraft, "Create Aircraft", "Users with this permission can create aircraft.")]
    AircraftCreate
}
```
The ```PermissionData``` attribute contains a category to link the permission to, a name, and a description.

```
public enum MyPermissionCategory : byte
{
    [CategoryData("Aircraft")]
    Aircraft
}
```
The ```CategoryData``` attribute contains a name for the category, if one isn't provided the name of the value is used (in this case there'd be no difference without the attribute).

## Evaluator 
You must provide an evaluator that takes the data used to actually make the decisison. All you have to do is implement the ```IAuthorizationEvaluator``` interface provided by the package. If you're using the single-tenant package, the tenant id parameter will be absent.
```
public class MyEvaluator : IAuthorizationEvaluator
{
    public async Task<AuthorizeDecision> EvaluateAsync(string userId, string tenantId, params string[] perms) { // determine if the user has the permissions and access to the tenant }
}
```

## Minimal application configuration
The application must be configured before and after the ```WebApplicationBuilder``` is built:

```Program.cs```
```
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddPermissionServer<MyPermission, MyPermissionCategory>()
    .AddAuthorizationEvaluator<MyEvaluator>();

var app = builder.ConfigureServices();
app.UsePermissionServer();
```

## Optional configuration
### Remote authorization
Use ```AddRemoteAuthorization()``` when you register PermissionServer. There are two parameters
- ```remoteAddress```: this is the url of either the protected resource such as an API, or the identity provider, depending on your situation.
- ```isAuthority```: Whether or not this is the identity provider that will evaluate the decision.

```Program.cs```
```
builder.Services.AddPermissionServer<MyPermission, MyPermissionCategory>()
    .AddAuthorizationEvaluator<MyEvaluator>()
    .AddRemoteAuthorization("https://localhost:5000", true); // localhost:5000 is an API
```

### EntityFramework management of permission and permission category
Use ```AddEntityFrameworkStores<TContext>``` when you register PermissionServer, and then make a call within your context's ```OnModelCreating()``` override.

```Program.cs```
```
builder.Services.AddPermissionServer<MyPermission, MyPermissionCategory>()
    .AddAuthorizationEvaluator<MyEvaluator>()
    .AddEntityFrameworkStores<MyDbContext>();
```

```MyDbContext.cs```
```
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.AddPermissionServer<PermissionEnum, PermissionCategoryEnum>();
}
```

Now apply migrations and generate your database normally. Note that changes to your enum values will affect the database, but only during migrations. Whenever migrations are applied, PermissionServer will synchronize these two enums to your database. It's up to you to ensure breaking changes are handled, for example cascading deletion of relational objects when a permission is deleted.



### Non-generic authorize attributes
If the usage of generic attributes is a nuisance or ugly to you, you can inherit from the attributes provided and not need to supply your enum type each call like so:
```
public class LocalAuthorizeAttribute : LocalAuthorizeAttribute<PermissionEnum>
{
    public LocalAuthorizeAttribute(params PermissionEnum[] permissions) : base(permissions) { }
}
```

You can now use this attribute without the generic parameter across your application.

### Custom route data identifiers
By default, the multi-tenant version of PermissionServer assumes your controllers have a route key with the name 'tenantId'. This is how the tenant id is obtained for each request. If you want to use this method but name your routedata key something else, this can be configured.

Similarly, by default the user id is grabbed from the 'sub' claim of your JWT. If you need it from, say, a 'userId' claim or something else, this can also be configured.

The following solves both issues
```Program.cs```
```
builder.Services.AddPermissionServer<PermissionEnum, PermissionCategoryEnum>(o =>
    {
        o.RouteDataTenantIdentifier = "myOtherIdentifier";
        o.JwtClaimUserIdentifier = "userId"
    });
```

The above routedata identifier would work with an attribute of e.g. ```[HttpGet(myendpoint/{myOtherIdentifier}/resource)]```.

### Custom user id and tenant id providers
If your tenant id and user id are obtained in other means, you can implement the interfaces and register them yourself, like so:
```Program.cs```
```
builder.Services.AddPermissionServer<PermissionEnum, PermissionCategoryEnum>()
    .AddTenantProvider<MyTenantProvider>()
    .AddUserProvider<MyUserProvider>();
```
the ```ITenantProvider``` and ```IUserProvider``` interfaces are provided in either packages.

# Multi-tenant package vs single-tenant package
The base package, PermissionServer, is multi-tenant. There is a separate package, PermissionServer.Singletenant, for normal usage.
