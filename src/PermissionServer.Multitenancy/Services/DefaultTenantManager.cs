using PermissionServer.Common.Results.Errors;
using PermissionServer.Multitenancy.Entities;

namespace PermissionServer.Multitenancy.Services
{
    public class DefaultTenantManager<TTenant, TUserTenant, TPerm, TPermCat>
        : ITenantManager<TTenant, TUserTenant, TPerm, TPermCat>
        where TTenant : PSTenant<TPerm, TPermCat>
        where TUserTenant : PSUserTenant<TPerm, TPermCat>
        where TPerm : Enum
        where TPermCat : Enum
    {
        #region TenantManagement
        IQueryable<TTenant> Tenants { get; }

        Task<TTenant> FindByIdAsync(Guid tenantId)
        {
            
        }

        Task CreateAsync(TTenant tenant, string ownerId);

        Task<TTenant> UpdateAsync(TTenant tenant);

        Task<bool> UserHasAccessAsync(Guid userId, Guid tenantId);

        Task<bool> ExistsAsync(Guid tenantId);
        #endregion
        #region UserManagement
        Task<List<TUserTenant>> GetUsersAsync(Guid tenantId);

        Task<List<TUserTenant>> GetUsersAwaitingApprovalAsync(Guid tenantId);

        Task<TUserTenant> FindUserByIdsAsync(Guid tenantId, Guid userId);

        public Task<Error> RevokeAccessAsync(Guid userId, Guid tenantId);

        Task<List<TUserTenant>> GetAllUsersTenantsAsync(Guid userId);
        #endregion
    }
}