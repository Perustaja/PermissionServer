using Microsoft.AspNetCore.Mvc;

namespace PermissionServer.Singletenant.Authorization
{
    /// <summary>
    /// Marks this method or class as requiring local authorization (i.e. user and permissions are stored
    /// locally within this project). 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method|AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class LocalAuthorizeAttribute : TypeFilterAttribute
    {
        /// <param name="permissions">A collection of permissions required.</param>
        public LocalAuthorizeAttribute(Enum[] permissions) : base(typeof(LocalAuthorizeFilter))
            => Arguments = new object[] { permissions };
    }
}