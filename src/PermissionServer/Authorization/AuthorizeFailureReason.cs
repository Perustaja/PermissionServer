namespace PermissionServer.Authorization
{
    public enum AuthorizeFailureReason
    {
        Unauthorized, // User does not have the required permissions
        PermissionFormat // The permissions passed were unsuccessfully parsed
    }
}