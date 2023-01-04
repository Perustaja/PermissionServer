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
                Action<PermissionServerOptions<TPerm, TPermCat>> options)
            where TPerm : Enum
            where TPermCat : Enum
        {
            if (sc == null)
                throw new ArgumentNullException(nameof(sc));

            sc.AddOptions();
            sc.Configure(options);
            sc.AddScoped<IPermissionService<TPerm, TPermCat>, DefaultPermissionService<TPerm, TPermCat>>();

            var b = new PermissionServerBuilder<TPerm, TPermCat>(sc);
            return b;
        }
    }
}