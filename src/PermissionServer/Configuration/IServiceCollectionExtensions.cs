using Microsoft.Extensions.DependencyInjection;

namespace PermissionServer.Configuration
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Sets up global roles to be seeded into the database. These are applied only during migrations.
        /// </summary>
        public static IServiceCollection AddGlobalRoles<TPerm, TPermCat>(
            this IServiceCollection sc,
            Action<GlobalRoleOptions<TPerm, TPermCat>> config)
            where TPerm : Enum
            where TPermCat : Enum
        {
            if (sc == null)
                throw new ArgumentNullException(nameof(sc));
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            return sc.Configure(config);
        }

        /// <summary>Adds PermissionServer with the default, single-tenant configuration.</summary>
        /// <returns>A <see cref="PermissionServerBuilder{T,K}"/> for configuring the authorization system.</returns>
        public static PermissionServerBuilder<TPerm, TPermCat> AddPermissionServer<TPerm, TPermCat>(this IServiceCollection sc)
            where TPerm : Enum
            where TPermCat : Enum
        {
            if (sc == null)
                throw new ArgumentNullException(nameof(sc));

            sc.AddOptions();
            var b = new PermissionServerBuilder<TPerm, TPermCat>(sc);
            return b;
        }
    }
}