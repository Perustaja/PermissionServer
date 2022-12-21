
namespace PermissionServer.Multitenancy.Authorization
{
    public interface IAuthorizationEvaluator
    {
        /// <summary>
        /// Determines if the current user has the specified permissions within the context of the 
        /// specified tenant. If no permissions are passed then whether or not the user has access
        /// to the specified tenant is checked.
        /// </summary>
        /// <param name="userId">The user id for the current request.</param>
        /// <param name="tenantId">The tenant id for the current request.</param>
        /// <param name="perms">Optional list of specified permissions obtained using Enum.ToString().</param>
        Task<AuthorizeDecision> EvaluateAsync(string userId, string tenantId, params string[] perms);
    }
}