using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using PermissionServer.Multitenancy.Configuration;
using PermissionServer.Multitenancy.Exceptions;

namespace PermissionServer.Multitenancy.Services
{
    public class RouteDataTenantProvider<TPerm, TPermCat> : ITenantProvider
        where TPerm : Enum
        where TPermCat : Enum
    {
        private readonly HttpContext _httpContext;
        private readonly MultitenantPermissionServerOptions<TPerm, TPermCat> _psOptions;

        public RouteDataTenantProvider(IHttpContextAccessor contextAccessor,
            IOptions<MultitenantPermissionServerOptions<TPerm, TPermCat>> psOptions)
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