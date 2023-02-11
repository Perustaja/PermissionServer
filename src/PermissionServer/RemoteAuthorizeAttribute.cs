using Microsoft.AspNetCore.Mvc;

namespace PermissionServer
{
    /// <summary>
    /// Marks this method or class as requiring remote authorization.
    /// </summary>
    /// <typeparam name="TPerm">The permission enum used when registering PermissionServer.</typeparam>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class RemoteAuthorizeAttribute<TPerm> : TypeFilterAttribute
        where TPerm : Enum
    {
        /// <param name="permissions">An optional collection of permissions required.</param>
        public RemoteAuthorizeAttribute(params TPerm[] permissions) : base(typeof(RemoteAuthorizeFilter<TPerm>))
            => Arguments = new object[] { false, String.Empty, permissions };

        /// <param name="authorityServerName">
        /// The name of the server registered when adding remote authorization. This is only used if you 
        /// have multiple authorities and have configured them when registering PermissionServer.
        /// </param>
        /// <param name="permissions">An optional collection of permissions required.</param>
        public RemoteAuthorizeAttribute(string authorityServerName, params TPerm[] permissions) 
            : base(typeof(RemoteAuthorizeFilter<TPerm>))
                => Arguments = new object[] { true, authorityServerName, permissions };
    }
}