namespace PermissionServer.Multitenancy.Services
{
    public interface IPermissionService<TPerm, TPermCat>
        where TPerm : Enum
        where TPermCat : Enum
    {
        public IQueryable<TPerm> Permissions { get; }
        public IQueryable<TPermCat> PermissionCategories { get; }
        
        /// <summary>
        /// Determines if the current user has the specified permissions within the context of the 
        /// specified tenant. If no permissions are passed then whether or not the user has access
        /// to the specified tenant is checked.
        /// </summary>
        /// <param name="userId">The user id for the current request.</param>
        /// <param name="tenantId">The tenant id for the current request.</param>
        /// <param name="perms">Optional list of specified permissions obtained using Enum.ToString().</param>
        Task<bool> UserHasPermissionsAsync(Guid userId, Guid tenantId, params string[] perms);

        /// <returns>
        /// A list of permissions that the user has within the tenant, or empty list if user, 
        /// tenant, or access record doesn't exist
        /// </returns>
        Task<List<TPerm>> GetUsersPermissionsAsync(Guid userId, Guid tenantId);
    }
}