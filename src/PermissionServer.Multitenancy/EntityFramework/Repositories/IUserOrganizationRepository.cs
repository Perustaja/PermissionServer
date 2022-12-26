using PermissionServer.Multitenancy.Entities;

namespace PermissionServer.Multitenancy.EntityFramework.Repositories
{
    public interface IUserTenantRepository<TUserTenant, TPerm, TPermCat>
        where TUserTenant : PSUserTenant<TPerm, TPermCat>
        where TPerm : Enum
        where TPermCat : Enum
    {
        Task<List<TUserTenant>> GetAllByTenantId(Guid tenantId);
        Task<List<TUserTenant>> GetAwaitingAccessByTenantId(Guid tenantId);
        Task<TUserTenant> GetByIdsAsync(Guid tenantId, Guid userId);
        Task<List<TUserTenant>> GetByUserIdAsync(Guid userId);
        TUserTenant Add(TUserTenant userTenant);
        TUserTenant Update(TUserTenant userTenant);
        void Delete(TUserTenant userTenant);
        Task<bool> ExistsWithAccessAsync(Guid userId, Guid tenantId);
    }
}