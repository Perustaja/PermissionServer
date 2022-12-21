namespace PermissionServer.Multitenancy.Services
{
    public interface ITenantManager<TPerm, TPermCat>
        where TPerm : Enum
        where TPermCat : Enum
    {
        /// <returns>Whether the user has access to the tenant, or false if either doesn't exist.</returns>
        Task<bool> UserHasAccessAsync(Guid userId, Guid orgId);
        /// <returns>Whether an Organization exists with the given id.</returns>
        Task<bool> ExistsAsync(Guid orgId);
    }
}