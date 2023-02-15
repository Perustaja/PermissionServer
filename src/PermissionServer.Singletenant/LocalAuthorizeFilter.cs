using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PermissionServer.Common;
using PermissionServer.Singletenant.Configuration;

namespace PermissionServer.Singletenant
{
    internal class LocalAuthorizeFilter<TPerm> : BaseAuthorizeFilter<TPerm>, IAsyncAuthorizationFilter
        where TPerm : Enum
    {
        public LocalAuthorizeFilter(TPerm[] permissions) : base(permissions) { }

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

            var evaulator = context.HttpContext.RequestServices
                .GetRequiredService(typeof(IAuthorizationEvaluator))
                as IAuthorizationEvaluator ?? throw new Exception("PermissionServer was unable to retrieve an instance of IAuthorizationFilter through DI. Ensure that if this is the authority, AddAuthorizationEvaluator() is called wherever you configure your services.");
            var userId = GetUserProvider(context.HttpContext).GetCurrentRequestUser().ToString();

            logger.LogInformation("Authorizing local request: {UserId}, {Permissions}", userId, Permissions);
            var decision = await evaulator.EvaluateAsync(userId, Permissions);
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
                        logger.LogCritical($"Unable to parse permissions from local attribute. {decision.FailureMessage}, {Permissions}");
                        context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
                        break;
                    default:
                        context.Result = new ForbidResult(); break;
                }
            }
        }
    }
}