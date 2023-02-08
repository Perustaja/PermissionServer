namespace PermissionServer.Singletenant
{
    public sealed class AuthorizeDecision
    {
        public bool Allowed { get; set; }
        public AuthorizeFailureReason? FailureReason { get; set; }
        public string FailureMessage { get; set; }
    }
}