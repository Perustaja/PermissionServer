namespace PermissionServer.Multitenancy.Services
{
    /// <summary>
    /// Provides access to the id of the current user for a request.
    /// </summary>
    public interface IUserProvider
    {
        /// <returns>The user id for the current request.</returns>
        /// <exception cref="Exceptions.UserNotFoundException">If user id is not found for current request.</exception>
        Guid GetCurrentRequestUser();
    }
}