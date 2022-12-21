using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Ps.Protobuf;

namespace PermissionServer.Multitenancy.Authorization
{
    public class RemoteAuthorizeFilter : BaseAuthorizeFilter, IAsyncAuthorizationFilter
    {
        public RemoteAuthorizeFilter(Enum[] permissions) : base(permissions) { }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var logger = GetLogger(context.HttpContext);

            if (context.HttpContext.User?.Identity.IsAuthenticated == false)
            {
                logger.LogWarning("User was not authenticated for authorization. Returning challenge.");
                context.Result = new ChallengeResult();
                return;
            }

            var client = GetService<GrpcPermissionAuthorize.GrpcPermissionAuthorizeClient>(context.HttpContext);
            var tenantId = GetTenantProvider(context.HttpContext).GetCurrentRequestTenant().ToString();
            var userId = GetUserProvider(context.HttpContext).GetCurrentRequestUser().ToString();

            var request = new GrpcPermissionAuthorizeRequest()
            {
                UserId = userId,
                TenantId = tenantId
            };

            if (_permissions != null)
                request.Perms.AddRange(_permissions);

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
                        logger.LogCritical("Identity server unable to parse permissions from attribute. {FailureMessage}, {Permissions}", reply.FailureMessage, _permissions);
                        context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
                        break;
                    case (failureReason.Tenantnotfound):
                        context.Result = new NotFoundResult(); break;
                    default:
                        context.Result = new ForbidResult(); break;
                }
            }
        }
    }
}