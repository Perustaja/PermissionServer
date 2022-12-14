namespace PermissionServer.Entities.Multitenancy
{
    /// <summary>
    /// Three-way join table that represents the roles a user has, specific to each tenant.
    /// </summary>
    public class PSUserTenantRole<TPerm, TPermCat>
        where TPerm : System.Enum
        where TPermCat : System.Enum
    {
        public Guid UserId { get; private set; }
        public Guid OrgId { get; private set; }
        public Guid RoleId { get; private set; }
        public PSUser<TPerm, TPermCat> User { get; set; }
        public PSTenant<TPerm, TPermCat> Tenant { get; set; }
        public PSRole<TPerm, TPermCat> Role { get; set; }
        public PSUserTenantRole() { }
        public PSUserTenantRole(Guid userId, Guid orgId, Guid roleId)
        {
            UserId = userId;
            OrgId = orgId;
            RoleId = roleId;
        }
    }
}