namespace PermissionServer.Entities.Multitenancy
{
    /// <summary>
    /// Three-way join table that represents the roles a user has, specific to each tenant.
    /// </summary>
    public class PSUserTenantRole<TPerm, TPermCat>
        where TPerm : Enum
        where TPermCat : Enum
    {
        public Guid UserId { get; set; }
        public Guid TenantId { get; set; }
        public Guid RoleId { get; set; }
        public PSUser<TPerm, TPermCat> User { get; set; }
        public PSTenant<TPerm, TPermCat> Tenant { get; set; }
        public PSRole<TPerm, TPermCat> Role { get; set; }

        public PSUserTenantRole() { }
        public PSUserTenantRole(Guid userId, Guid tenantId, Guid roleId)
        {
            UserId = userId;
            TenantId = tenantId;
            RoleId = roleId;
        }
    }
}