using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace PermissionServer
{
    public sealed class TenantNotFoundException : Exception
    {
        public readonly string _customMessage;
        public override string Message => _customMessage;
        /// <summary>
        /// Represents an exception where the tenant information is sourced through HttpContext RouteData,
        /// and the expected key was not found.
        /// </summary>
        public TenantNotFoundException(HttpContext requestContext, string expectedKey)
        {
            string temp =
            $"Unable to source tenant id from RouteData {requestContext.TraceIdentifier}: Expected key: {expectedKey} Actual kvps: ";
            foreach (var kvp in requestContext.GetRouteData().Values)
                temp += (kvp.Key + ":" + kvp.Value + "\n");
            _customMessage = temp; 
        }
    }
}