using PermissionServer.Common.Results.Errors;
using PermissionServer.Multitenancy.Entities;

namespace PermissionServer.Multitenancy.Services
{
    /// <summary>Allows for tenants and their users' access records to be managed.</summary>
    public interface ITenantManager<TTenant, TUserTenant, TPerm, TPermCat>
        where TTenant : PSTenant<TPerm, TPermCat>
        where TUserTenant : PSUserTenant<TPerm, TPermCat>
        where TPerm : Enum
        where TPermCat : Enum
    {
        #region TenantManagement
        /// <value>An IQueryable of the registered tenant type.</value>
        IQueryable<TTenant> Tenants { get; }

        /// <returns>The tenant with the corresponding id, or null.</returns>
        Task<TTenant> FindByIdAsync(Guid tenantId);

        /// <summary>Initializes a new tenant with the given user as the owner.</summary>
        Task CreateAsync(TTenant tenant, string ownerId);

        /// <summary>Updates the given tenant.</summary>
        /// <returns>The tenant entity being tracked after update.</returns>
        Task<TTenant> UpdateAsync(TTenant tenant);

        /// <returns>Whether the user has access to the tenant, or false if either doesn't exist.</returns>
        Task<bool> UserHasAccessAsync(Guid userId, Guid tenantId);

        /// <returns>Whether a tenant exists with the given id.</returns>
        Task<bool> ExistsAsync(Guid tenantId);
        #endregion
        #region UserManagement
        /// <returns>
        /// A list containing an access record for each user with access to the tenant, or empty list if tenant non-existent.
        /// </returns>
        Task<List<TUserTenant>> GetUsersAsync(Guid tenantId);

        /// <returns>
        /// A list of access records awaiting approval or empty list if tenant not found 
        /// or no users awaiting approval.
        /// </returns>
        Task<List<TUserTenant>> GetUsersAwaitingApprovalAsync(Guid tenantId);

        /// <returns>
        /// A tenant access record, or null if user doesn't have access to the tenant.
        /// </returns>
        Task<TUserTenant> FindUserByIdsAsync(Guid tenantId, Guid userId);

        /// <summary>Attempts to revoke the user's access to the tenant.</summary>
        /// <returns>An <see cref="Error"/> if the user is the owner or does not have access.</returns>
        public Task<Error> RevokeAccessAsync(Guid userId, Guid tenantId);

        /// <returns>
        /// All tenant access records for a user, representing all of the tenants they 
        /// have access too, or empty list if none found.
        /// </returns>
        Task<List<TUserTenant>> GetAllUsersTenantsAsync(Guid userId);
        #endregion
    }
}