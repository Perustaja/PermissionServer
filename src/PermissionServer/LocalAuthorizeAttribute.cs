using Microsoft.AspNetCore.Mvc;

namespace PermissionServer
{
    /// <summary>
    /// Marks this method or class as requiring local authorization (i.e. user, tenant, and permissions 
    /// are stored locally within this project).
    /// </summary>
    /// <typeparam name="TPerm">The permission enum used when registering PermissionServer.</typeparam>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class LocalAuthorizeAttribute<TPerm> : TypeFilterAttribute
        where TPerm : Enum
    {
        /// <param name="permissions">An optional collection of permissions required.</param>
        public LocalAuthorizeAttribute(params TPerm[] permissions) : base(typeof(LocalAuthorizeFilter<TPerm>))
            => Arguments = new object[] { permissions };
    }
}