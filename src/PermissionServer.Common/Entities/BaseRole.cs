using Microsoft.AspNetCore.Identity;

namespace PermissionServer.Common.Entities
{
    /// <summary>Base class without navigation properties for polymorphism in dbcontext.</summary>
    public class BaseRole<TPerm, TPermCat> : IdentityRole<Guid>
        where TPerm : Enum
        where TPermCat : Enum
    {
        public string Description { get; set; }
        /// <summary>Whether or not the role is the default for a new user.</summary>
        public bool IsGlobalDefaultForNewUsers { get; set; }

        public BaseRole() { }

        public BaseRole(string name, string desc)
        {
            Name = name;
            NormalizedName = name.ToUpper();
            Description = desc;
        }
    }
}