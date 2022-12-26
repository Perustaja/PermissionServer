namespace PermissionServer.Multitenancy.EntityFramework.Repositories
{
    internal interface IPermissionRepository<TPerm, TPermCat>
        where TPerm : Enum
        where TPermCat : Enum
    {
        Task<bool> UserHasPermissionsAsync(Guid userId, Guid tenantId, string[] perms);
        Task<List<TPerm>> GetUsersPermissionsAsync(Guid userId, Guid orgId);
        Task<List<TPerm>> GetAllPermissionsAsync();
        Task<List<TPermCat>> GetAllPermissionCategoriesAsync();
    }
}