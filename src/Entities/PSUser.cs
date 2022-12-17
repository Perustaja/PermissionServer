using Microsoft.AspNetCore.Identity;

namespace PermissionServer.Entities
{
    /// <summary>
    /// Base entity for a user. 
    /// </summary>
    public class PSUser<TPerm, TPermCat> : IdentityUser<Guid>
        where TPerm : Enum
        where TPermCat : Enum
    {
        public List<PSUserRole<TPerm, TPermCat>> UserRoles { get; set; }
        
        public PSUser() { }
        public PSUser(string email)
        {
            Id = Guid.NewGuid();
            UserName = email;
            Email = email;
        }
    }
}