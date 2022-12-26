using PermissionServer.Multitenancy.Entities;

namespace PermissionServer.Multitenancy.EntityFramework.Repositories
{
    public interface IUserOrganizationRoleRepository<TUserTenantRole, TRole, TPerm, TPermCat>
        where TUserTenantRole : PSUserTenantRole<TPerm, TPermCat>
        where TRole : PSRole<TPerm, TPermCat>
        where TPerm : Enum
        where TPermCat : Enum
    {
        TUserTenantRole Add(TUserTenantRole userTenantRole);
        void Update(List<TUserTenantRole> uors);
        void Delete(TUserTenantRole uor);
        Task<bool> RoleIsOnlyRoleForAnyUserAsync(TRole role);
        Task<List<TUserTenantRole>> GetUsersRolesAsync(Guid userId, Guid tenantId);
    }
}