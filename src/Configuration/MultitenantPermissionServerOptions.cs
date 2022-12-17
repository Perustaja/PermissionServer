namespace PermissionServer.Configuration
{
    public class MultitenantPermissionServerOptions<TPerm, TPermCat>
        where TPerm : Enum
        where TPermCat : Enum
    {
        public string TenantIdentifier { get; set; } = "tenantId";
    }
}
