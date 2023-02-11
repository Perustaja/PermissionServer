using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PermissionServer.Common.Internal;

namespace PermissionServer.Common
{
    public abstract class BaseAuthorizeFilter<TPerm>
        where TPerm : Enum
    {
        protected readonly string[] Permissions;
        public BaseAuthorizeFilter(TPerm[] permissions)
        {
            Permissions = permissions.Select(p => p.ToString()).ToArray<string>();
        }

        // It seems like there is still no easy way to use DI with action filters in 6.0,
        // figuring out how to do this in a more normal way would be nice but is low prio
        protected ILogger<BaseAuthorizeFilter<TPerm>> GetLogger(HttpContext context)
            => GetService<ILogger<BaseAuthorizeFilter<TPerm>>>(context);

        protected IUserProvider GetUserProvider(HttpContext context)
            => GetService<IUserProvider>(context);

        protected T GetService<T>(HttpContext context)
            where T : class     
        {
            return context.RequestServices
                .GetRequiredService(typeof(T))
                as T 
                ?? throw new Exception($"PermissionServer was unable to retrieve an instance of {typeof(T)} through DI.");
        }
    }
}