namespace PermissionServer.Multitenancy.Configuration
{
    internal interface IAuthoritySettings
    {
        bool IsAuthority();
    }

    internal sealed class AuthoritySettings : IAuthoritySettings
    {
        private bool _isAuthority;
        public AuthoritySettings(bool isAuthority) => _isAuthority = isAuthority;
        public bool IsAuthority() => _isAuthority;
    }
}