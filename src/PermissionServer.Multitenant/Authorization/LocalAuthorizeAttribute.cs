using Microsoft.AspNetCore.Mvc;

namespace PermissionServer.Multitenant.Authorization
{
    /// <summary>
    /// Marks this method or class as requiring local authorization (i.e. user, tenant, and permissions 
    /// are stored locally within this project). 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class LocalAuthorizeAttribute : TypeFilterAttribute
    {
        /// <param name="permissions">An optional collection of permissions required.</param>
        public LocalAuthorizeAttribute(params Enum[] permissions) : base(typeof(LocalAuthorizeFilter))
            => Arguments = new object[] { permissions };
    }
}