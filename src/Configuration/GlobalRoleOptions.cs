using PermissionServer.Entities;

namespace PermissionServer.Configuration
{
    public class GlobalRoleOptions<TPerm, TPermCat>
        where TPerm : Enum
        where TPermCat : Enum
    {
        private List<GlobalRoleBuilder<TPerm, TPermCat>> _builders 
            = new List<GlobalRoleBuilder<TPerm, TPermCat>>();
        public List<PSRole<TPerm, TPermCat>> Roles => _builders.Select(b => b.BuildRole()).ToList();
        public List<PSRolePermission<TPerm, TPermCat>> RolePermissions => _builders.SelectMany(b => b.BuildPermissions()).ToList();
        public void AddGlobalRole(Action<GlobalRoleBuilder<TPerm, TPermCat>> ba)
        {
            var b = new GlobalRoleBuilder<TPerm, TPermCat>();
            ba?.Invoke(b);
            _builders.Add(b);
        }
    }
}