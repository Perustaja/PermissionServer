using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace PermissionServer.Multitenancy.Authorization
{
    public class LocalAuthorizeFilter : BaseAuthorizeFilter, IAsyncAuthorizationFilter
    {
        public LocalAuthorizeFilter(Enum[] permissions) : base(permissions) { }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var logger = GetLogger(context.HttpContext);
            logger.LogInformation("Beginning local authorization request.");

            if (context.HttpContext.User?.Identity.IsAuthenticated == false)
            {
                logger.LogWarning("User was not authenticated for authorization. Returning challenge.");
                context.Result = new ChallengeResult();
                return;
            }

            var evaulator = GetService<IAuthorizationEvaluator>(context.HttpContext);
            var tenantId = GetTenantProvider(context.HttpContext).GetCurrentRequestTenant().ToString();
            var userId = GetUserProvider(context.HttpContext).GetCurrentRequestUser().ToString();



            logger.LogInformation("Authorizing local request: {UserId}, {TenantId}, {Permissions}", userId, tenantId, _permissions);
            var decision = await evaulator.EvaluateAsync(userId, tenantId, _permissions);
            SetContextResultOnDecision(context, decision);
        }

        protected void SetContextResultOnDecision(AuthorizationFilterContext context, AuthorizeDecision decision)
        {
            var logger = GetLogger(context.HttpContext);
            logger.LogInformation("Authorization result: {Decision}", decision);
            if (!decision.Allowed)
            {
                switch (decision.FailureReason)
                {
                    case (AuthorizeFailureReason.PermissionFormat):
                        logger.LogCritical($"Unable to parse permissions from local attribute. {decision.FailureMessage}, {_permissions}");
                        context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
                        break;
                    case (AuthorizeFailureReason.TenantNotFound):
                        context.Result = new NotFoundResult(); break;
                    default:
                        context.Result = new ForbidResult(); break;
                }
            }
        }
    }
}