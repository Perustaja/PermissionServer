using PermissionServer.Entities.Multitenancy;

namespace PermissionServer.Configuration
{
    public class MultitenantEntityOptions<TPerm, TPermCat>
        where TPerm : Enum
        where TPermCat : Enum
    {
        internal Type RolePermissionType { get; set; } = typeof(PSRolePermission<TPerm, TPermCat>);
        internal Type TenantType { get; set; } = typeof(PSTenant<TPerm, TPermCat>);
        internal Type UserTenantType { get; set; } = typeof(PSUserTenant<TPerm, TPermCat>);
        internal Type UserTenantRoleType { get; set; } = typeof(PSUserTenantRole<TPerm, TPermCat>);

        public MultitenantEntityOptions<TPerm, TPermCat> AddCustomRolePermission<TRolePerm>()
            where TRolePerm : PSRolePermission<TPerm, TPermCat>
        {
            RolePermissionType = typeof(TRolePerm);
            return this;
        }

        public MultitenantEntityOptions<TPerm, TPermCat> AddCustomTenant<TTenant>()
            where TTenant : PSTenant<TPerm, TPermCat>
        {
            TenantType = typeof(TTenant);
            return this;
        }

        public MultitenantEntityOptions<TPerm, TPermCat> AddCustomUserTenant<TUserTenant>()
            where TUserTenant : PSUserTenant<TPerm, TPermCat>
        {
            RolePermissionType = typeof(TUserTenant);
            return this;
        }

        public MultitenantEntityOptions<TPerm, TPermCat> AddCustomUserTenantRole<TUserTenantRole>()
            where TUserTenantRole : PSUserTenantRole<TPerm, TPermCat>
        {
            RolePermissionType = typeof(TUserTenantRole);
            return this;
        }
    }
}