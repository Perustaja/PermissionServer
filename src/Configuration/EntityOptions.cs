using PermissionServer.Entities;

namespace PermissionServer.Configuration
{
    public class EntityOptions<TPerm, TPermCat>
        where TPerm : Enum
        where TPermCat : Enum
    {
        internal Type RolePermissionType { get; set; } = typeof(PSRolePermission<TPerm, TPermCat>);
        internal Type UserRoleType { get; set; } = typeof(PSUserRole<TPerm, TPermCat>);

        public EntityOptions<TPerm, TPermCat> AddCustomRolePermission<TRolePerm>()
            where TRolePerm : PSRolePermission<TPerm, TPermCat>
        {
            RolePermissionType = typeof(TRolePerm);
            return this;
        }

        public EntityOptions<TPerm, TPermCat> AddCustomUserRole<TUserRole>()
            where TUserRole : PSUserRole<TPerm, TPermCat>
        {
            UserRoleType = typeof(TUserRole);
            return this;
        }
    }
}