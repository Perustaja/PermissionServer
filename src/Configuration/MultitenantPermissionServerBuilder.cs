using Microsoft.Extensions.DependencyInjection;

namespace PermissionServer.Configuration
{
    public class MultitenantPermissionServerBuilder<TPerm, TPermCat>
        where TPerm : Enum
        where TPermCat : Enum
    {
        private IServiceCollection Services { get; }
        public MultitenantPermissionServerBuilder(IServiceCollection services,
            Action<MultitenantPermissionServerOptions<TPerm, TPermCat>> oa)
        {
            Services = services;
            Services.Configure(oa);
        }

        /// <summary>Adds remote authorization to the application.</summary>
        /// <param name="isAuthority">Whether this is the server that will evaluate authorization decisions.</param>
        /// <param name="remoteAddress">
        /// The url of the remote server, e.g. "https://localhost:5000", If this is the authority,
        /// put in the url of the client and vice-versa.
        /// </param>
        /// <returns>The current <see cref="MultitenantPermissionServerBuilder{T,K}"/> instance.</returns>
        public MultitenantPermissionServerBuilder<TPerm, TPermCat> AddRemoteAuthorization(bool isAuthority,
            string remoteAddress)
        {
            // make sure remoteAddress isnt empty
            // add grpc stuff based on multitenancy status and based on authority or client
            return this;
        }

        /// <summary>Adds global roles to be seeded into the database during migrations.</summary>
        /// <param name="oa">An action to configure the <see cref="MultitenantGlobalRolesOptions{T,K}"/>.</param>
        /// <returns>The current <see cref="MultitenantPermissionServerBuilder{T,K}"/> instance.</returns>
        public MultitenantPermissionServerBuilder<TPerm, TPermCat> AddSeededGlobalRoles(
            Action<MultitenantGlobalRoleOptions<TPerm, TPermCat>> oa)
        {
            Services.Configure(oa);
            return this;
        }

        public MultitenantPermissionServerBuilder<TPerm, TPermCat> AddCustomEntities()
        {
            return this;
        }
    }
}