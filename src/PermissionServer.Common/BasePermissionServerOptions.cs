namespace PermissionServer.Common
{
    public class BasePermissionServerOptions
    {
        internal Type PermissionEnumType { get; set; }
        internal Type PermissionCategoryEnumType { get; set; }

        /// <summary>
        /// The key in the JWT that corresponds to the user id for the current request.
        /// Default value is "sub".
        /// </summary>
        public string JwtClaimUserIdentifier { get; set; } = "sub";
    }
}
