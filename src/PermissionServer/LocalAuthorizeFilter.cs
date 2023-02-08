using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PermissionServer.Common;

namespace PermissionServer
{
    public class LocalAuthorizeFilter<TPerm> : BaseAuthorizeFilter<TPerm>, IAsyncAuthorizationFilter
        where TPerm : Enum
    {
        public LocalAuthorizeFilter(TPerm[] permissions) : base(permissions) { }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var registeredEnumType 
                = GetService<IOptions<PermissionServerOptions>>(context.HttpContext).Value.PermissionEnumType;
            ValidateUserProvidedEnum(registeredEnumType);
            
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
            var tenantId = GetService<ITenantProvider>(context.HttpContext).GetCurrentRequestTenant().ToString();
            var userId = GetUserProvider(context.HttpContext).GetCurrentRequestUser().ToString();

            logger.LogInformation("Authorizing local request: {UserId}, {TenantId}, {Permissions}", userId, tenantId, Permissions);
            var decision = await evaulator.EvaluateAsync(userId, tenantId, Permissions);
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
                    case (AuthorizeFailureReason.TenantNotFound):
                        context.Result = new NotFoundResult(); break;
                    default:
                        context.Result = new ForbidResult(); break;
                }
            }
        }
    }
}