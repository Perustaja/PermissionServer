using Microsoft.Extensions.Options;
using PermissionServer.Entities.Multitenancy;

namespace PermissionServer.Configuration.Multitenancy
{
    public class DefaultGlobalRoleProvider<TPerm, TPermCat> : IGlobalRoleProvider<TPerm, TPermCat>
        where TPerm : System.Enum
        where TPermCat : System.Enum
    {
        private readonly GlobalRolesOptions<TPerm, TPermCat> _options;
        public DefaultGlobalRoleProvider(IOptions<GlobalRolesOptions<TPerm, TPermCat>> options)
        {   
            _options = options.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public List<PSRole<TPerm, TPermCat>> GetGlobalRoles() => _options.Roles;
        public List<PSRolePermission<TPerm, TPermCat>> GetGlobalRolePermissions() => _options.RolePermissions;
    }
}