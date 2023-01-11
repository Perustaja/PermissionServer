namespace PermissionServer.Multitenancy.Configuration
{
    internal interface IAuthoritySettings
    {
        public bool IsAuthority { get; }
    }

    internal sealed class AuthoritySettings : IAuthoritySettings
    {
        public bool IsAuthority { get; }
        public AuthoritySettings(bool isAuthority) => IsAuthority = isAuthority;
    }
}