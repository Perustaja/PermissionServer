using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PermissionServer.Multitenancy.Configuration;
using PermissionServer.Multitenancy.Exceptions;
using PermissionServer.Multitenancy.Services;

namespace PermissionServer.Multitenancy.Authorization
{
    internal abstract class BaseAuthorizeFilter
    {
        private readonly IEnumerable<Type> _enumTypes;
        protected readonly string[] Permissions;
        public BaseAuthorizeFilter(Enum[] permissions)
        {
            _enumTypes = permissions.Select(p => p.GetType());
            Permissions = permissions.Select(p => p.ToString()).ToArray<string>();
        }

        // It seems like there is still no easy way to use DI with action filters in 6.0,
        // figuring out how to do this in a more normal way would be nice but is low prio
        protected ITenantProvider GetTenantProvider(HttpContext context)
            => GetService<ITenantProvider>(context);

        protected ILogger<BaseAuthorizeFilter> GetLogger(HttpContext context)
            => GetService<ILogger<BaseAuthorizeFilter>>(context);

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

        protected void ValidateUserProvidedEnum(Type registeredEnumType)
        {
            var possibleWrongEnum =_enumTypes.FirstOrDefault(t => t != registeredEnumType);
            if (possibleWrongEnum != null)
                throw new AttributeArgumentException(registeredEnumType, possibleWrongEnum);
        }
    }
}