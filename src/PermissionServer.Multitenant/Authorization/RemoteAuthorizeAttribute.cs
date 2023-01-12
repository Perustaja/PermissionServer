using Microsoft.AspNetCore.Mvc;

namespace PermissionServer.Multitenant.Authorization
{
    /// <summary>
    /// Marks this method or class as requiring remote authorization.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class RemoteAuthorizeAttribute : TypeFilterAttribute
    {
        /// <param name="permissions">An optional collection of permissions required.</param>
        public RemoteAuthorizeAttribute(params Enum[] permissions) : base(typeof(RemoteAuthorizeAttribute))
            => Arguments = new object[] { permissions };
    }
}