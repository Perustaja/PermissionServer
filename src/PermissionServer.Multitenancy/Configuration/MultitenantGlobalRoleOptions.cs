using PermissionServer.Multitenancy.Entities;

namespace PermissionServer.Multitenancy.Configuration
{
    public class MultitenantGlobalRoleOptions<TPerm, TPermCat>
        where TPerm : Enum
        where TPermCat : Enum
    {
        private List<MultitenantGlobalRoleBuilder<TPerm, TPermCat>> _builders 
            = new List<MultitenantGlobalRoleBuilder<TPerm, TPermCat>>();
        public List<PSRole<TPerm, TPermCat>> Roles => _builders.Select(b => b.BuildRole()).ToList();
        public List<PSRolePermission<TPerm, TPermCat>> RolePermissions => _builders.SelectMany(b => b.BuildPermissions()).ToList();
        public void AddGlobalRole(Action<MultitenantGlobalRoleBuilder<TPerm, TPermCat>> ba)
        {
            var b = new MultitenantGlobalRoleBuilder<TPerm, TPermCat>();
            ba?.Invoke(b);
            _builders.Add(b);
        }
    }
}