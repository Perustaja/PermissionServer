using PermissionServer.Multitenancy.Entities;

namespace PermissionServer.Multitenancy.EntityFramework.Repositories
{
    internal interface ITenantRepository<TTenant, TPerm, TPermCat>
        where TTenant : PSTenant<TPerm, TPermCat>
        where TPerm : Enum
        where TPermCat : Enum
    {
        Task<bool> ExistsAsync(Guid id);
        Task<TTenant> FindByIdAsync(Guid id);
        TTenant Create(TTenant tenant);
        TTenant Update(TTenant tenant);
        void Delete(TTenant tenant);
    }
}