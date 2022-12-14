using PermissionServer.Entities.Multitenancy;

namespace PermissionServer.Configuration.Multitenancy
{
    public interface IGlobalRoleProvider<TPerm, TPermCat>
        where TPerm : System.Enum
        where TPermCat : System.Enum
    {
        /// <returns>A List containing all global roles for the application.</returns>
        public List<PSRole<TPerm, TPermCat>> GetGlobalRoles();
        
        /// <returns>A List containing all permissions of a global role for the application.</returns>
        public List<PSRolePermission<TPerm, TPermCat>> GetGlobalRolePermissions();
    }
}