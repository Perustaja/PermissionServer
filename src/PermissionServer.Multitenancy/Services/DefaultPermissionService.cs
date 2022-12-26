namespace PermissionServer.Multitenancy.Services
{
    public class DefaultPermissionService<TPerm, TPermCat> : IPermissionService<TPerm, TPermCat>
        where TPerm : Enum
        where TPermCat : Enum
    {
        public Task<bool> UserHasPermissionsAsync(Guid userId, Guid tenantId, params string[] perms)
        {
            throw new NotImplementedException();
        }
    }
}