namespace PermissionServer.Configuration
{
    public class RemoteAuthorizationOptions
    {
        public bool isAuthority { get; set; }
        public Uri RemoteAddress { get; set; }
    }
}