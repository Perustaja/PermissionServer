
namespace PermissionServer.Singletenant
{
    public interface IAuthorizationEvaluator
    {
        /// <summary>Determines if the current user has the specified permissions</summary>
        /// <param name="userId">The user id for the current request.</param>
        /// <param name="permissions">Specified permissions obtained using Enum.ToString().</param>
        Task<AuthorizeDecision> EvaluateAsync(string userId, string[] permissions);
    }
}