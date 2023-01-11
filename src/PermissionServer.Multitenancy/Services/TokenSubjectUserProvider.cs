using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using PermissionServer.Multitenancy.Configuration;
using PermissionServer.Multitenancy.Exceptions;

namespace PermissionServer.Multitenancy.Services
{
    public class TokenSubjectUserProvider : IUserProvider
    {
        private readonly HttpContext _httpContext;
        private readonly PermissionServerOptions _psOptions;

        public TokenSubjectUserProvider(IHttpContextAccessor contextAccessor,
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