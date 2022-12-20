using Microsoft.AspNetCore.Identity;

namespace PermissionServer.Entities.Bases
{
    /// <summary>Base class without navigation properties for polymorphism in dbcontext.</summary>
    public abstract class BaseUser<TPerm, TPermCat> : IdentityUser<Guid>
        where TPerm : Enum
        where TPermCat : Enum
    {
        public BaseUser() { }
        public BaseUser(string email)
        {
            Id = Guid.NewGuid();
            UserName = email;
            Email = email;
        }
    }
}