using Microsoft.Extensions.DependencyInjection;
using PermissionServer.Multitenancy.Services;

namespace PermissionServer.Multitenancy.Configuration
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>Adds PermissionServer with the default, multi-tenant configuration without gRPC.</summary>
        /// <typeparam name="TPerm">The enum which represents your application's permissions.</typeparam>
        /// <returns>A <see cref="PermissionServerBuilder"/> for configuring the authorization system.</returns>
        public static PermissionServerBuilder<TPerm, TPermCat> AddPermissionServer<TPerm, TPermCat>(this IServiceCollection sc,
            Action<PermissionServerOptions> options)
                where TPerm : Enum
                where TPermCat : Enum
        {
            if (sc == null)
                throw new ArgumentNullException(nameof(sc));

            var addTypes = new Action<PermissionServerOptions>(o => 
            {
                o.PermissionEnumType = typeof(TPerm);
                o.PermissionCategoryEnumType = typeof(TPermCat);
            });

            sc.AddOptions();
            sc.Configure(options + addTypes);
            sc.AddScoped<ITenantProvider, RouteDataTenantProvider>();
            sc.AddScoped<IUserProvider, TokenSubjectUserProvider>();

            var b = new PermissionServerBuilder<TPerm, TPermCat>(sc);
            return b;
        }
    }
}