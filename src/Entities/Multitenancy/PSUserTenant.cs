namespace PermissionServer.Entities.Multitenancy
{
    /// <summary>
    /// Join table that acts as an access record.
    /// </summary>
    public class PSUserTenant<TPerm, TPermCat>
        where TPerm : System.Enum
        where TPermCat : System.Enum
    {
        public Guid UserId { get; private set; }
        public Guid OrgId { get; private set; }
        public PSUser<TPerm, TPermCat> User { get; set; }
        public PSTenant<TPerm, TPermCat> Tenant { get; set; }
        public PSUserTenant() { }
        public PSUserTenant(Guid userId, Guid orgId)
        {
            UserId = userId;
            OrgId = orgId;
        }
    }
}