namespace PermissionServer.Multitenancy.Entities
{
    /// <summary>
    /// Join table that acts as an access record.
    /// </summary>
    public class PSUserTenant<TPerm, TPermCat>
        where TPerm : Enum
        where TPermCat : Enum
    {
        public Guid UserId { get; set; }
        public Guid TenantId { get; set; }
        public PSUser<TPerm, TPermCat> User { get; set; }
        public PSTenant<TPerm, TPermCat> Tenant { get; set; }

        public PSUserTenant() { }
        public PSUserTenant(Guid userId, Guid tenantId)
        {
            UserId = userId;
            TenantId = tenantId;
        }
    }
}