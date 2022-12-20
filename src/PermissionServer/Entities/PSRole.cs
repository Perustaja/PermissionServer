using PermissionServer.Common.Entities;

namespace PermissionServer.Entities
{
    /// <summary>
    /// Defines a group of permissions and some configuration values. 
    /// </summary
    public class PSRole<TPerm, TPermCat> : BaseRole<TPerm, TPermCat>
        where TPerm : Enum
        where TPermCat : Enum
    {
        /// <summary>Whether the role is the default role given to new users upon registration.</summary>
        public bool IsDefaultForNewUsers { get; set; }
        public List<PSRolePermission<TPerm, TPermCat>> RolePermissions { get; set; }

        public PSRole() { }

        public PSRole(string name, string desc) : base(name, desc) {}

        public void SetAsDefaultNewUserRole() => IsDefaultForNewUsers = true;
    }
}