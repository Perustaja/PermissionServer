using Microsoft.Extensions.DependencyInjection;
using Ps.Protobuf;

namespace PermissionServer.Multitenancy.Configuration
{
    public sealed class PermissionServerBuilder<TPerm, TPermCat>
        where TPerm : Enum
        where TPermCat : Enum
    {
        private IServiceCollection Services { get; }
        public PermissionServerBuilder(IServiceCollection services)
        {
            Services = services;
        }

        /// <summary>Adds remote authorization to the application.</summary>
        /// <param name="isAuthority">Whether this is the server that will evaluate authorization decisions.</param>
        /// <param name="remoteAddress">
        /// The url of the remote server, e.g. "https://localhost:5000", If this is the authority,
        /// put in the url of the client and vice-versa.
        /// </param>
        /// <returns>The current <see cref="PermissionServerBuilder{T,K}"/> instance.</returns>
        public PermissionServerBuilder<TPerm, TPermCat> AddRemoteAuthorization(string remoteAddress,
            bool isAuthority)
        {
            if (String.IsNullOrEmpty(remoteAddress))
                throw new Exception("Remote address cannot be null.");

            Services.AddGrpc();
            Services.AddSingleton<IAuthoritySettings>(new AuthoritySettings(isAuthority));
            if (isAuthority)
            {
                Services.AddGrpcClient<GrpcPermissionAuthorize.GrpcPermissionAuthorizeBase>(o =>
                {
                    o.Address = new Uri(remoteAddress);
                });
            }
            else
            {
                Services.AddGrpcClient<GrpcPermissionAuthorize.GrpcPermissionAuthorizeClient>(o =>
                {
                    o.Address = new Uri(remoteAddress);
                });
            }

            return this;
        }

        /// <summary>Adds global roles to be seeded into the database during migrations.</summary>
        /// <param name="options">An action to configure the <see cref="GlobalRolesOptions{T,K}"/>.</param>
        /// <returns>The current <see cref="PermissionServerBuilder{T,K}"/> instance.</returns>
        public PermissionServerBuilder<TPerm, TPermCat> AddSeededGlobalRoles(
            Action<GlobalRoleOptions<TPerm, TPermCat>> options)
        {
            Services.Configure(options);
            return this;
        }

        public PermissionServerBuilder<TPerm, TPermCat> AddCustomEntities(
            Action<EntityOptions<TPerm, TPermCat>> options)
        {
            Services.Configure(options);
            return this;
        }
    }
}