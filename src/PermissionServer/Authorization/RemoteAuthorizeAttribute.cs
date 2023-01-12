using Microsoft.AspNetCore.Mvc;

namespace PermissionServer.Authorization
{
    /// <summary>
    /// Marks this method or class as requiring remote authorization.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method|AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class RemoteAuthorizeAttribute : TypeFilterAttribute
    {
        /// <param name="permissions">A collection of permissions required.</param>
        public RemoteAuthorizeAttribute(Enum[] permissions) : base(typeof(RemoteAuthorizeAttribute))
            => Arguments = new object[] { permissions };
    }
}