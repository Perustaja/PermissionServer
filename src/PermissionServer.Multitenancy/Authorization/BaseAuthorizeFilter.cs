using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PermissionServer.Multitenancy.Services;

namespace PermissionServer.Multitenancy.Authorization
{
    public abstract class BaseAuthorizeFilter
    {
        protected readonly string[] _permissions;
        public BaseAuthorizeFilter(Enum[] permissions)
        {
            _permissions = permissions.Select(p => p.ToString()).ToArray<string>();
        }

        // It seems like there is still no easy way to use DI with action filters in 6.0,
        // figuring out how to do this in a more normal way would be nice but is low prio
        protected ITenantProvider GetTenantProvider(HttpContext context)
            => GetService<ITenantProvider>(context);

        protected ILogger<LocalAuthorizeFilter> GetLogger(HttpContext context)
            => GetService<ILogger<LocalAuthorizeFilter>>(context);

        protected IUserProvider GetUserProvider(HttpContext context)
            => GetService<IUserProvider>(context);

        protected T GetService<T>(HttpContext context)
            where T : class     
        {
            return context.RequestServices
                .GetRequiredService(typeof(T))
                as T;
        }
    }
}