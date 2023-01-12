using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using PermissionServer.Common.Configuration;
using PermissionServer.Common.Exceptions;

namespace PermissionServer.Common.Services
{
    public class TokenUserProvider : IUserProvider
    {
        private readonly HttpContext _httpContext;
        private readonly PermissionServerOptions _psOptions;

        public TokenUserProvider(IHttpContextAccessor contextAccessor,
            IOptions<PermissionServerOptions> psOptions)
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