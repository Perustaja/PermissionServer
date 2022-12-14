using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PermissionServer.Configuration.Multitenancy;

namespace PermissionServer.Configuration
{
    public static class IServiceCollectionExtensions
    {
        private static IServiceCollection AddGlobalRoles<TPerm, TPermCat>(this IServiceCollection sc)
            where TPerm : System.Enum
            where TPermCat : System.Enum
        {
            sc.AddOptions();
            sc.TryAdd(ServiceDescriptor.Transient<IGlobalRoleProvider<TPerm, TPermCat>, DefaultGlobalRoleProvider<TPerm, TPermCat>>());
            return sc;
        }

        /// <summary>
        /// Sets up global roles to be seeded into the database with migrations.
        /// </summary>
        public static IServiceCollection AddGlobalRoles<TPerm, TPermCat>(
            this IServiceCollection sc,
            Action<GlobalRolesOptions<TPerm, TPermCat>> config)
            where TPerm : System.Enum
            where TPermCat : System.Enum
        {
            if (sc == null)
                throw new ArgumentNullException(nameof(sc));
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            sc.Configure(config);
            return sc.AddGlobalRoles<TPerm, TPermCat>();
        }
    }
}