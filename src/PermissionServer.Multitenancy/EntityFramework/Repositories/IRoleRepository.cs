using PermissionServer.Multitenancy.Entities;

namespace PermissionServer.Multitenancy.EntityFramework.Repositories
{
    public interface IRoleRepository<TRole, TPerm, TPermCat>
        where TRole : PSRole<TPerm, TPermCat>
        where TPerm : Enum
        where TPermCat : Enum
    {
        Task<List<TRole>> GetRolesByTenantIdAsync(Guid tenantId);
        Task<TRole> GetRoleOfTenantByIdsAsync(Guid tenantId, Guid roleId);
        Task<TRole> GetGlobalDefaultOwnerRoleAsync();
        Task<TRole> GetGlobalDefaultNewUserRoleAsync();
        TRole Add(Guid tenantId, TRole role);
        TRole Update(TRole role);
        void Delete(TRole role);
    }
}