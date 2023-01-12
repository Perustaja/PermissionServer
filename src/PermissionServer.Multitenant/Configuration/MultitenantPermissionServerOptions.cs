using PermissionServer.Common.Configuration;

namespace PermissionServer.Multitenant.Configuration
{
    public sealed class MultitenantPermissionServerOptions : PermissionServerOptions
    {
        /// <summary>
        /// The route parameter key on your controller methods that contains the tenant id 
        /// for the current request. e.g. [HttpGet("tenants/{tenantId}/users")].
        /// Default value is "tenantId".
        /// </summary>
        public string RouteDataTenantIdentifier { get; set; } = "tenantId";
    }
}
