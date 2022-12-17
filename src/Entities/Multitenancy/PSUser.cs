using Microsoft.AspNetCore.Identity;

namespace PermissionServer.Entities.Multitenancy
{
    /// <summary>
    /// Base entity for a user. 
    /// </summary>
    public class PSUser<TPerm, TPermCat> : IdentityUser<Guid>
        where TPerm : Enum
        where TPermCat : Enum
    {
        public List<PSUserTenant<TPerm, TPermCat>> UserTenants { get; set; }
        public List<PSUserTenantRole<TPerm, TPermCat>> UserTenantRoles { get; set; }
        
        public PSUser() { }
        public PSUser(string email)
        {
            Id = Guid.NewGuid();
            UserName = email;
            Email = email;
        }
    }
}