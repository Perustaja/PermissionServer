using Microsoft.AspNetCore.Mvc;

namespace PermissionServer
{
    /// <summary>
    /// Marks this method or class as requiring remote authorization.
    /// </summary>
    /// <typeparam name="TPerm">The permission enum used when registering PermissionServer.</typeparam>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class RemoteAuthorizeAttribute<TPerm> : TypeFilterAttribute
        where TPerm : Enum
    {
        /// <param name="permissions">An optional collection of permissions required.</param>
        public RemoteAuthorizeAttribute(params TPerm[] permissions) : base(typeof(RemoteAuthorizeFilter<TPerm>))
            => Arguments = new object[] { permissions };
    }
}