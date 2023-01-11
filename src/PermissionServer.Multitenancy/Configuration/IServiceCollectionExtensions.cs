using Microsoft.Extensions.DependencyInjection;
using PermissionServer.Multitenancy.Services;

namespace PermissionServer.Multitenancy.Configuration
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>Adds PermissionServer with the default, multi-tenant configuration without gRPC.</summary>
        /// <returns>A <see cref="PermissionServerBuilder{T,K}"/> for configuring the authorization system.</returns>
        public static PermissionServerBuilder<TPerm, TPermCat> AddPermissionServer<TPerm, TPermCat>(
                this IServiceCollection sc,
                Action<PermissionServerOptions> options)
            where TPerm : Enum
            where TPermCat : Enum
        {
            if (sc == null)
                throw new ArgumentNullException(nameof(sc));

            var configuredOpts = new PermissionServerOptions();
            options?.Invoke(configuredOpts);
            configuredOpts.PermissionEnumType = typeof(TPerm);

            sc.AddOptions();
            sc.Configure(options);
            sc.AddScoped<ITenantProvider, RouteDataTenantProvider>();
            sc.AddScoped<IUserProvider, TokenSubjectUserProvider>();

            var b = new PermissionServerBuilder<TPerm, TPermCat>(sc);
            return b;
        }
    }
}