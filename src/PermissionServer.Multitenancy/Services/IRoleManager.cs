using PermissionServer.Common.Results.Errors;
using PermissionServer.Multitenancy.Entities;

namespace PermissionServer.Multitenancy.Services
{
    /// <summary>Allows for tenant-specific roles to be managed, including assigning them to users and 
    /// revoking them from users.
    /// </summary>
    public interface IRoleManager<TRole, TPerm, TPermCat>
        where TRole : PSRole<TPerm, TPermCat>
        where TPerm : Enum
        where TPermCat : Enum
    {
        /// <returns>A list of roles including tenant-specific and global roles.</returns>
        Task<List<TRole>> GetRolesAsync(Guid tenantId);

        /// <returns>The corresponding role within the tenant, or null if not found.</returns>
        Task<TRole> FindRoleByIdsAsync(Guid tenantId, Guid roleId);

        /// <summary>Adds a role to be used by the specified tenant with the given permissions, if any.</summary>
        /// <returns>The role entity being tracked after add.</returns>
        Task<TRole> CreateRoleAsync(Guid tenantId, TRole role, params TPerm[] perms);

        /// <summary>Updates the given role.</summary>
        /// <returns>The role entity being tracked after update.</returns>
        Task UpdateRoleAsync(Guid tenantId, TRole role);

        /// <summary>Attempts to delete the role.</summary>
        /// <returns>An <see cref="Error"/> if the role is global, or the only role of any user.</returns>
        Task<Error> DeleteRoleOfOrgAsync(TRole role);

        /// <summary>Attempts to remove the role.</summary>
        /// <returns>An <see cref="Error"/> if this is the user's last role.</returns>
        Task<Error> RemoveRoleFromUserAsync(Guid userId, Guid tenantId, Guid roleId);
    }
}