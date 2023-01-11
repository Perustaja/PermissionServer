using Microsoft.Extensions.DependencyInjection;
using PermissionServer.Multitenancy.Authorization;
using PermissionServer.Multitenancy.Services;
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
                throw new ArgumentNullException("Remote address cannot be null or empty.");

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

        /// <summary>Adds a custom evaluator that determines authorization decisions.</summary>
        /// /// <returns>The current <see cref="PermissionServerBuilder{T,K}"/> instance.</returns>
        public PermissionServerBuilder<TPerm, TPermCat> AddAuthorizationEvaluator<TEvaluator>()
            where TEvaluator : class, IAuthorizationEvaluator
        {
            Services.AddScoped<IAuthorizationEvaluator, TEvaluator>();
            return this;
        }

        /// <summary>Adds a custom user provider that obtains the user for each request.</summary>
        /// /// <returns>The current <see cref="PermissionServerBuilder{T,K}"/> instance.</returns>
        public PermissionServerBuilder<TPerm, TPermCat> AddUserProvider<TProvider>()
            where TProvider : class, IUserProvider
        {
            RemovePossibleDefault<TProvider>();
            Services.AddScoped<IUserProvider, TProvider>();
            return this;
        }

        /// <summary>Adds a custom tenant provider that obtains the user for each request.</summary>
        /// /// <returns>The current <see cref="PermissionServerBuilder{T,K}"/> instance.</returns>
        public PermissionServerBuilder<TPerm, TPermCat> AddTenantProvider<TProvider>()
            where TProvider : class, ITenantProvider
        {
            RemovePossibleDefault<TProvider>();
            Services.AddScoped<ITenantProvider, TProvider>();
            return this;
        }

        private void RemovePossibleDefault<TInterface>()
        {
            var possibleDefault =
                Services.FirstOrDefault(d => d.ServiceType == typeof(IUserProvider));
            if (possibleDefault != default(ServiceDescriptor))
                Services.Remove(possibleDefault);
        }
    }
}