using PermissionServer.Entities.Bases;

namespace PermissionServer.Entities.Multitenancy
{
    /// <summary>
    /// Multitenant user entity.
    /// </summary>
    public class PSUser<TPerm, TPermCat> : BaseUser<TPerm, TPermCat>
        where TPerm : Enum
        where TPermCat : Enum
    {
        public List<PSUserTenant<TPerm, TPermCat>> UserTenants { get; set; }
        public List<PSUserTenantRole<TPerm, TPermCat>> UserTenantRoles { get; set; }

        public PSUser() { }
    }
}