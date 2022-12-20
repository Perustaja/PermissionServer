using Microsoft.Extensions.DependencyInjection;

namespace PermissionServer.Multitenancy.Configuration
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>Adds PermissionServer with the default, multi-tenant configuration.</summary>
        /// <returns>A <see cref="MultitenantPermissionServerBuilder{T,K}"/> for configuring the authorization system.</returns>
        public static MultitenantPermissionServerBuilder<TPerm, TPermCat> AddPermissionServerWithMultitenancy<TPerm, TPermCat>(
                this IServiceCollection sc,
                Action<MultitenantPermissionServerOptions<TPerm, TPermCat>> oa
            )
            where TPerm : Enum
            where TPermCat : Enum
        {
            if (sc == null)
                throw new ArgumentNullException(nameof(sc));

            sc.AddOptions();
            // register tenant provider middleware
            var b = new MultitenantPermissionServerBuilder<TPerm, TPermCat>(sc, oa);
            return b;
        }
    }
}