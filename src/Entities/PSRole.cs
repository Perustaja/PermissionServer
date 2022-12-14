using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace PermissionServer.Entities
{
    /// <summary>
    /// Defines a group of permissions and some configuration values. 
    /// </summary
    public class PSRole<TPerm, TPermCat> : IdentityRole<Guid>
        where TPerm : System.Enum
        where TPermCat : System.Enum
    {
        public string Description { get; private set; }
        /// <summary>Whether the role is the default role given to new users upon registration.</summary>
        public bool IsDefaultForNewUsers { get; private set; }
        [ForeignKey("TenantId")]
        public List<PSRolePermission<TPerm, TPermCat>> RolePermissions { get; set; }
        public PSRole() { }

        /// <summary>
        /// Creates a new role.
        /// </summary>
        public PSRole(string name, string desc)
        {
            Name = name;
            NormalizedName = name.ToUpper();
            Description = desc;
        }

        public void SetAsDefaultNewUserRole() => IsDefaultForNewUsers = true;
    }
}