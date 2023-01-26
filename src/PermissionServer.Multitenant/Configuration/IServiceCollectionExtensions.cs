using Microsoft.Extensions.DependencyInjection;
using PermissionServer.Common.Services;
using PermissionServer.Multitenant.Services;

namespace PermissionServer.Multitenant.Configuration
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>Adds PermissionServer with the default, multi-tenant configuration without gRPC.</summary>
        /// <typeparam name="TPerm">The enum which represents your application's permissions.</typeparam>
        /// <typeparam name="TPermCat">The enum which represents your application's permission categories.</typeparam>
        /// <returns>A <see cref="MultitenantPermissionServerBuilder{T,K}"/> for configuring the authorization system.</returns>
        public static MultitenantPermissionServerBuilder<TPerm, TPermCat> AddPermissionServer<TPerm, TPermCat>(
            this IServiceCollection sc)
                where TPerm : Enum
                where TPermCat : Enum
            => AddPermissionServer<TPerm, TPermCat>(sc, options: null);

        /// <summary>Adds PermissionServer with the default, multi-tenant configuration without gRPC.</summary>
        /// <typeparam name="TPerm">The enum which represents your application's permissions.</typeparam>
        /// <typeparam name="TPermCat">The enum which represents your application's permission categories.</typeparam>
        /// <returns>A <see cref="MultitenantPermissionServerBuilder{T,K}"/> for configuring the authorization system.</returns>
        public static MultitenantPermissionServerBuilder<TPerm, TPermCat> AddPermissionServer<TPerm, TPermCat>(
            this IServiceCollection sc,
            Action<MultitenantPermissionServerOptions> options)
                where TPerm : Enum
                where TPermCat : Enum
        {
            if (sc == null)
                throw new ArgumentNullException(nameof(sc));

            var addTypes = new Action<MultitenantPermissionServerOptions>(o =>
            {
                o.PermissionEnumType = typeof(TPerm);
                o.PermissionCategoryEnumType = typeof(TPermCat);
            });

            sc.AddOptions();
            sc.Configure(options + addTypes);
            sc.AddScoped<ITenantProvider, RouteDataTenantProvider>();
            sc.AddScoped<IUserProvider, TokenUserProvider>();

            var b = new MultitenantPermissionServerBuilder<TPerm, TPermCat>(sc);
            return b;
        }
    }
}