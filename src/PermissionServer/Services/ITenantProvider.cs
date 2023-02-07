using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using PermissionServer.Configuration;
using PermissionServer.Exceptions;

namespace PermissionServer.Services
{
    /// <summary>Provides access to the information of the tenant for a request.</summary>
    public interface ITenantProvider
    {
        /// <returns>The tenant id for the current request.</returns>
        /// <exception cref="Exceptions.TenantNotFoundException">If tenant id is not found for current request.</exception>
        Guid GetCurrentRequestTenant();
    }

    public class RouteDataTenantProvider : ITenantProvider
    {
        private readonly HttpContext _httpContext;
        private readonly PermissionServerOptions _psOptions;

        public RouteDataTenantProvider(IHttpContextAccessor contextAccessor,
            IOptions<PermissionServerOptions> psOptions)
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