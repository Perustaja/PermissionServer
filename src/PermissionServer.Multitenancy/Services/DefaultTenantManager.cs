namespace PermissionServer.Multitenancy.Services
{
    public class DefaultTenantManager<TPerm, TPermCat> : ITenantManager<TPerm, TPermCat>
        where TPerm : Enum
        where TPermCat : Enum
    {
        public Task<bool> ExistsAsync(Guid orgId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UserHasAccessAsync(Guid userId, Guid orgId)
        {
            throw new NotImplementedException();
        }
    }
}