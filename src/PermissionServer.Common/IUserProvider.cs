using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using PermissionServer.Common.Internal;

namespace PermissionServer.Common
{
    /// <summary>
    /// Provides access to the id of the current user for a request.
    /// </summary>
    public interface IUserProvider
    {
        /// <returns>The user id for the current request.</returns>
        /// <exception cref="Internal.UserNotFoundException">If user id is not found for current request.</exception>
        Guid GetCurrentRequestUser();
    }

    public class TokenUserProvider : IUserProvider
    {
        private readonly HttpContext _httpContext;
        private readonly BasePermissionServerOptions _psOptions;

        public TokenUserProvider(IHttpContextAccessor contextAccessor,
            IOptions<BasePermissionServerOptions> psOptions)
        {
            _httpContext = contextAccessor.HttpContext ?? throw new ArgumentNullException(nameof(contextAccessor.HttpContext));
            _psOptions = psOptions.Value;
        }

        public Guid GetCurrentRequestUser()
        {
            string userId = _httpContext.User.FindFirstValue(_psOptions.JwtClaimUserIdentifier);
            if (userId == null)
                throw new UserNotFoundException(_httpContext, _psOptions.JwtClaimUserIdentifier);
            return new Guid(userId);
        }
    }
}