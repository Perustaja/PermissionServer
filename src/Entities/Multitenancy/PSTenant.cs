namespace PermissionServer.Entities.Multitenancy
{
    /// <summary>
    /// Base entity for a tenant. A tenant has its own user-defined roles along with global defaults.
    /// </summary>
    public class PSTenant<TPerm, TPermCat>
        where TPerm : System.Enum
        where TPermCat : System.Enum
    {
        public Guid Id { get; private set; }
        public Guid OwnerUserId { get; private set; }
        public string Title { get; private set; }
        public bool IsActive { get; private set; }
        public List<PSUserTenant<TPerm, TPermCat>> UserTenants { get; set; }
        public List<PSUserTenantRole<TPerm, TPermCat>> UserTenantRoles { get; set; }
        public PSTenant() { }
        public PSTenant(string title, Guid ownerId)
        {
            Id = new Guid();
            Title = title;
            IsActive = true;
            OwnerUserId = ownerId;
        }
    }
}