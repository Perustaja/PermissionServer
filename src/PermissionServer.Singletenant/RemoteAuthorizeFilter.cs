using Grpc.Net.ClientFactory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PermissionServer.Common;
using PermissionServer.Singletenant.Configuration;
using Ps.Protobuf;

namespace PermissionServer.Singletenant
{
    internal class RemoteAuthorizeFilter<TPerm> : BaseAuthorizeFilter<TPerm>, IAsyncAuthorizationFilter
        where TPerm : Enum
    {
        private readonly bool UsesMultipleAuths;
        private readonly string ServerName;
        public RemoteAuthorizeFilter(bool usesMultipleAuths, string serverName, TPerm[] permissions) 
            : base(permissions) 
            { 
                if (usesMultipleAuths && String.IsNullOrEmpty(serverName))
                    throw new ArgumentNullException("Server name cannot be null or empty in remote authorize attribute.");
                UsesMultipleAuths = usesMultipleAuths;
                ServerName = serverName;
            }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var logger = GetLogger(context.HttpContext);

            if (context.HttpContext.User?.Identity.IsAuthenticated == false)
            {
                logger.LogWarning("User was not authenticated for authorization. Returning challenge.");
                context.Result = new ChallengeResult();
                return;
            }

            var userId = GetUserProvider(context.HttpContext).GetCurrentRequestUser().ToString();
            var client = (UsesMultipleAuths) 
                ? GetService<GrpcClientFactory>(context.HttpContext)
                    .CreateClient<GrpcPermissionAuthorize.GrpcPermissionAuthorizeClient>(ServerName)
                : GetService<GrpcPermissionAuthorize.GrpcPermissionAuthorizeClient>(context.HttpContext); 

            var request = new GrpcPermissionAuthorizeRequest()
            {
                UserId = userId,
            };

            if (Permissions != null)
                request.Permissions.AddRange(Permissions);

            logger.LogInformation("Authorization request to be sent via GRPC: {Request}", request);
            var reply = await client.AuthorizeAsync(request);
            SetContextResultOnReply(context, reply);
        }

        private void SetContextResultOnReply(AuthorizationFilterContext context, GrpcAuthorizeDecision reply)
        {
            var logger = GetLogger(context.HttpContext);
            logger.LogInformation("Remote authorization result: {Reply}", reply);
            if (!reply.Allowed)
            {
                switch (reply.FailureReason)
                {
                    case (failureReason.Permissionformat):
                        logger.LogCritical("Identity server unable to parse permissions from attribute. {FailureMessage}, {Permissions}", reply.FailureMessage, Permissions);
                        context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
                        break;
                    default:
                        context.Result = new ForbidResult(); break;
                }
            }
        }
    }
}