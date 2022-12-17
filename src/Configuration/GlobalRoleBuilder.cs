using PermissionServer.Entities;

namespace PermissionServer.Configuration
{
    /// <summary>
    /// Configures a single global role to be seeded into the database. Start WithBaseRole() then add 
    /// permissions and change configuration as desired.
    /// </summary>
    public class GlobalRoleBuilder<TPerm, TPermCat>
        where TPerm : Enum
        where TPermCat : Enum
    {
        private PSRole<TPerm, TPermCat> _role;
        private HashSet<PSRolePermission<TPerm, TPermCat>> _rolePermissions;
        public PSRole<TPerm, TPermCat> BuildRole() => _role;
        public HashSet<PSRolePermission<TPerm, TPermCat>> BuildPermissions() => _rolePermissions;
        public GlobalRoleBuilder<TPerm, TPermCat> WithBaseRole(string name, string desc)
        {
            _rolePermissions = new HashSet<PSRolePermission<TPerm, TPermCat>>();
            _role = new PSRole<TPerm, TPermCat>(name, desc);
            return this;
        }

        public GlobalRoleBuilder<TPerm, TPermCat> AsDefaultNewUserRole()
        {
            ensureBaseRoleCreated();
            _role.SetAsDefaultNewUserRole();
            return this;
        }

        public GlobalRoleBuilder<TPerm, TPermCat> GrantAllPermissions()
        {
            ensureBaseRoleCreated();
            foreach (TPerm p in Enum.GetValues(typeof(TPerm)))
                _rolePermissions.Add(new PSRolePermission<TPerm, TPermCat>(_role.Id, p));
            return this;
        }

        public GlobalRoleBuilder<TPerm, TPermCat> GrantPermissions(params TPerm[] perms)
        {
            ensureBaseRoleCreated();
            foreach (TPerm p in perms)
                _rolePermissions.Add(new PSRolePermission<TPerm, TPermCat>(_role.Id, p));
            return this;
        }

        public GlobalRoleBuilder<TPerm, TPermCat> GrantAllPermissionsExcept(params TPerm[] perms)
        {
            ensureBaseRoleCreated();
            foreach (TPerm p in Enum.GetValues(typeof(TPerm)))
                if (!perms.Any(perm => EqualityComparer<TPerm>.Default.Equals(perm, p)))
                    _rolePermissions.Add(new PSRolePermission<TPerm, TPermCat>(_role.Id, p));
            return this;
        }

        private void ensureBaseRoleCreated()
        {
            if (_role == null || _rolePermissions == null)
                throw new Exception("Attempted to configure a global role without specifying a base role. Ensure WithBaseRole() is called before further configuration.");
        }
    }
}