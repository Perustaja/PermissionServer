namespace PermissionServer.Multitenancy.Configuration
{
    public sealed class PermissionServerOptions
    {
        internal Type PermissionEnumType { get; set; }
        internal Type PermissionCategoryEnumType { get; set; }

        /// <summary>
        /// The route parameter key on your controller methods that contains the tenant id 
        /// for the current request. e.g. [HttpGet("tenants/{tenantId}/users")].
        /// Default value is "tenantId".
        /// </summary>
        public string RouteDataTenantIdentifier { get; set; } = "tenantId";

        /// <summary>
        /// The key in the JWT that corresponds to the user id for the current request.
        /// Default value is "sub".
        /// </summary>
        public string JwtClaimUserIdentifier { get; set; } = "sub";

        /// <summary>
        /// If this is set to true, the evaluator will not check if each permission in each
        /// request is a defined type of your configured permission enum. This is available in case 
        /// Enum.IsDefined() is acting as a performance hindrance and the value of a more explicit
        /// runtime error is not necessary when the wrong enum type is used in the authorization attributes.
        /// Default value is false.
        /// </summary>
        public bool DisableEvaluatorEnumTypeChecks { get; set; } = false;
    }
}
