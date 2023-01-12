using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using PermissionServer.Multitenant.Configuration;
using PermissionServer.Multitenant.Exceptions;

namespace PermissionServer.Multitenant.Services
{
    public class RouteDataTenantProvider : ITenantProvider
    {
        private readonly HttpContext _httpContext;
        private readonly MultitenantPermissionServerOptions _psOptions;

        public RouteDataTenantProvider(IHttpContextAccessor contextAccessor,
            IOptions<MultitenantPermissionServerOptions> psOptions)
        {
            _httpContext = contextAccessor.HttpContext ?? throw new ArgumentNullException(nameof(contextAccessor.HttpContext));
            _psOptions = psOptions.Value;
        }

        public Guid GetCurrentRequestTenant()
        {
            _httpContext.GetRouteData().Values.TryGetValue(_psOptions.RouteDataTenantIdentifier, out object value);
            if (value == null)
                throw new TenantNotFoundException(_httpContext, _psOptions.RouteDataTenantIdentifier);
            return new Guid(value.ToString());
        }
    }
}