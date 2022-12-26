namespace PermissionServer.Common.Results.Errors
{
    public class Error
    {
        /// <summary>Represents an error specific to PermissionServer, the description contains a message
        /// safe to show to end-users explaining the issue with the intended operation. e.g. a non-global
        /// role was attempted to be deleted but was assigned to a user as their only role, so a user
        /// would be left role-less.
        /// </summary>
        /// <param name="desc">An end-user formatted message explaining the issue with the operation.</param>
        public Error(string desc) => Description = desc;

        /// <value>An end-user formatted message explaining the issue with the operation.</value>
        public string Description { get; private set; }
    }
}