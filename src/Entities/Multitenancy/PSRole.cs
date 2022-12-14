using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace PermissionServer.Entities.Multitenancy
{
    /// <summary>
    /// Defines a group of permissions and some configuration values. A role may be specific to a tenant, 
    /// or may be global and shared as a non-removable default value across tenants.
    /// </summary>
    public class PSRole<TPerm, TPermCat> : IdentityRole<Guid>
        where TPerm : System.Enum
        where TPermCat : System.Enum
    {
        public Guid? TenantId { get; private set; }
        public string Description { get; private set; }
        public bool IsGlobal { get; private set; }
        /// <summary>Whether or not the role is the default for a new tenant owner.</summary>
        public bool IsGlobalAdminDefault { get; private set; }
        /// <summary>Whether or not the role is the default for a new user.</summary>
        public bool IsGlobalDefaultForNewUsers { get; private set; }
        /// <summary>
        /// Whether or not the role is the default for a new user for this tenant. Takes priority 
        /// over global.
        /// </summary>
        public bool IsTenantDefaultForNewUsers { get; private set; }
        [ForeignKey("TenantId")]
        public PSTenant<TPerm, TPermCat> Tenant { get; set; }
        public List<PSRolePermission<TPerm, TPermCat>> RolePermissions { get; set; }
        public List<PSUserTenantRole<TPerm, TPermCat>> UserOrganizationRoles { get; set; }
        public PSRole() { }

        /// <summary>
        /// Creates a new role specific to a tenant.
        /// </summary>
        public PSRole(string name, string desc)
        {
            Name = name;
            NormalizedName = name.ToUpper();
            IsGlobal = false;
            Description = desc;
            IsGlobalAdminDefault = false;
            IsGlobalDefaultForNewUsers = false;
            IsTenantDefaultForNewUsers = false;
        }

        ///<returns>A global role accessible by all tenants to be tracked by migrations.</returns>
        public static PSRole<TPerm, TPermCat> SeededGlobalRole(string name, string desc)
        {
            var r = new PSRole<TPerm, TPermCat>();
            r.IsGlobal = true;
            r.Name = name;
            r.NormalizedName = name.ToUpper();
            r.Description = desc;
            r.IsGlobalAdminDefault = false;
            r.IsGlobalDefaultForNewUsers = false;
            r.IsTenantDefaultForNewUsers = false;
            return r;
        }

        /// <summary>Configures this role to be assigned to all new tenant owners within the application.</summary>
        public void SetAsGlobalAdminRole()
        {
            if (IsGlobal)
                IsGlobalAdminDefault = true;
        }

        public void SetAsTenantDefaultNewUserRole()
        {
            if (!IsGlobal)
                IsTenantDefaultForNewUsers = true;
        }

        public void SetAsGlobalDefaultNewUserRole()
        {
            if (IsGlobal)
                IsGlobalDefaultForNewUsers = true;
        }

        /// <summary>
        /// Sets this Role as belonging to the given Organization. Silently fails if this Role is global.
        /// </summary>
        public void SetTenant(Guid tid)
        {
            if (!IsGlobal)
                TenantId = tid;
        }
    }
}