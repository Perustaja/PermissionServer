using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace PermissionServer.Multitenancy.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public readonly string _customMessage;
        public override string Message => _customMessage;
        /// <summary>
        /// Represents an exception where the user id is sourced by taking the subject claim from the JWT
        /// and no valid guid was found.
        /// </summary>
        public UserNotFoundException(HttpContext requestContext, string expectedClaim)
        {
            string temp =
            $"Unable to source user id from token {requestContext.TraceIdentifier}: Expected claim: {expectedClaim} Actual claims: ";
            foreach (var c in requestContext.User.Claims)
                temp += (c.Type + ":" + c.Value + "\n");
            _customMessage = temp; 
        }
    }
}