namespace PermissionServer.Multitenancy.Configuration
{
    public class MultitenantPermissionServerOptions<TPerm, TPermCat>
        where TPerm : Enum
        where TPermCat : Enum
    {
        public string TenantIdentifier { get; set; } = "tenantId";
    }
}
