using Microsoft.AspNetCore.Identity;

namespace PermissionServer.Entities
{
    public class User : IdentityUser<Guid>
    {
        public User() { }
        public User(string fName, string lName, string email)
        {
            Id = Guid.NewGuid();
            UserName = email;
            Email = email;
        }
    }
}