namespace PermissionServer.Multitenancy.Services
{
    /// <summary>
    /// Provides access to the information of the tenant for a request.
    /// </summary>
    public interface ITenantProvider
    {
        /// <returns>The tenant id for the current request.</returns>
        /// <exception cref="Exceptions.TenantNotFoundException">If tenant id is not found for current request.</exception>
        Guid GetCurrentRequestTenant();
    }
}