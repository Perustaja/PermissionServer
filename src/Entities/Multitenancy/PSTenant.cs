namespace PermissionServer.Entities.Multitenancy
{
    /// <summary>
    /// Base entity for a tenant. A tenant has its own user-defined roles along with global defaults.
    /// </summary>
    public class PSTenant<TPerm, TPermCat>
        where TPerm : Enum
        where TPermCat : Enum
    {
        public Guid Id { get; set; }
        public Guid OwnerUserId { get; set; }
        public string Title { get; set; }
        public bool IsActive { get; set; }
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