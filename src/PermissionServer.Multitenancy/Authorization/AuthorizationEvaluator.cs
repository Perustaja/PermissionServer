using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PermissionServer.Multitenancy.Configuration;
using PermissionServer.Multitenancy.Services;

namespace PermissionServer.Multitenancy.Authorization
{
    public class AuthorizationEvaluator<TPerm, TPermCat> : IAuthorizationEvaluator
        where TPerm : Enum
        where TPermCat : Enum
    {
        private readonly ILogger<AuthorizationEvaluator<TPerm, TPermCat>> _logger;
        private readonly ITenantManager<TPerm, TPermCat> _tenantManager;
        private readonly IPermissionService<TPerm, TPermCat> _permSvc;
        private readonly MultitenantPermissionServerOptions<TPerm, TPermCat> _psOptions;

        public AuthorizationEvaluator(ILogger<AuthorizationEvaluator<TPerm, TPermCat>> logger,
            ITenantManager<TPerm, TPermCat> tenantManager,
            IPermissionService<TPerm, TPermCat> permSvc,
            IOptions<MultitenantPermissionServerOptions<TPerm, TPermCat>> psOptions)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _tenantManager = tenantManager ?? throw new ArgumentNullException(nameof(tenantManager));
            _permSvc = permSvc ?? throw new ArgumentNullException(nameof(permSvc));
            _psOptions = psOptions.Value;
        }
        public async Task<AuthorizeDecision> EvaluateAsync(string userId, string orgId, params string[] perms)
        {
            var permsSet = new HashSet<string>(perms);
            if (!_psOptions.DisableEvaluatorEnumTypeChecks)
            {
                foreach (string s in permsSet)
                {
                    // the string value will be used, but ensure that it maps to an actual permission now.
                    // this check could potentially be a bottle neck as it will be called very frequently,
                    // see https://www.mariuszwojcik.com/enums-parsing-performance/ for a remedy (that or
                    // this check could be removed since the attributes have type safety.)
                    if (!Enum.IsDefined(typeof(TPerm), s))
                    {
                        _logger.LogError($"Unable to parse string: {s} to PermissionEnum.");
                        return new AuthorizeDecision()
                        {
                            Allowed = false,
                            FailureReason = AuthorizeFailureReason.PermissionFormat,
                            FailureMessage = $"Unable to parse {s} to PermissionEnum."
                        };
                    }

                }
            }

            Guid userIdGuid = new Guid(userId);
            Guid orgIdGuid = new Guid(orgId);
            // Check if Organization exists so a 404 can be returned on request for non-existent org
            if (!await _tenantManager.ExistsAsync(orgIdGuid))
            {
                _logger.LogInformation($"Request was for non-existant or inactive tenant: {orgId}");
                return new AuthorizeDecision() { Allowed = false, FailureReason = AuthorizeFailureReason.TenantNotFound };
            }

            // If no permissions specified, simply check whether the User has access
            // If permissions specified, ensure user has all and access in one query
            if (permsSet.Count == 0 && await _tenantManager.UserHasAccessAsync(userIdGuid, orgIdGuid))
            {
                _logger.LogInformation($"Authorization successful with no permission checks. user:{userIdGuid} org:{orgIdGuid}.");
                return Ok();
            }
            else if (permsSet.Count > 0 && await _permSvc.UserHasPermissionsAsync(userIdGuid, orgIdGuid, permsSet.ToArray()))
            {
                _logger.LogInformation("Authorization successful. user:{UserId} org:{OrgId} {Permissions}.", userIdGuid, orgIdGuid, permsSet);
                return Ok();
            }

            _logger.LogInformation("Authorization failed. user:{UserId} org:{OrgId} {Permissions}.", userIdGuid, orgIdGuid, permsSet);
            return UnAuthorized();
        }

        private AuthorizeDecision UnAuthorized()
            => new AuthorizeDecision() { Allowed = false, FailureReason = AuthorizeFailureReason.Unauthorized };
        private AuthorizeDecision Ok()
            => new AuthorizeDecision() { Allowed = true };
    }
}