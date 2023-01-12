using Microsoft.Extensions.DependencyInjection;
using PermissionServer.Common.Configuration;
using PermissionServer.Multitenant.Authorization;
using PermissionServer.Multitenant.Services;
using Ps.Protobuf;

namespace PermissionServer.Multitenant.Configuration
{
    public sealed class MultitenantPermissionServerBuilder<TPerm, TPermCat> : PermissionServerBuilder<TPerm, TPermCat>
        where TPerm : Enum
        where TPermCat : Enum
    {
        private IServiceCollection Services { get; }
        public MultitenantPermissionServerBuilder(IServiceCollection services)
        : base(services) { }

        /// <summary>Adds remote authorization to the application.</summary>
        /// <param name="isAuthority">Whether this is the server that will evaluate authorization decisions.</param>
        /// <param name="remoteAddress">
        /// The url of the remote server, e.g. "https://localhost:5000", If this is the authority,
        /// put in the url of the client and vice-versa.
        /// </param>
        /// <returns>The current <see cref="MultitenantPermissionServerBuilder{T,K}"/> instance.</returns>
        public MultitenantPermissionServerBuilder<TPerm, TPermCat> AddRemoteAuthorization(string remoteAddress,
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
        /// /// <returns>The current <see cref="MultitenantPermissionServerBuilder{T,K}"/> instance.</returns>
        public MultitenantPermissionServerBuilder<TPerm, TPermCat> AddAuthorizationEvaluator<TEvaluator>()
            where TEvaluator : class, IAuthorizationEvaluator
        {
            Services.AddScoped<IAuthorizationEvaluator, TEvaluator>();
            return this;
        }

        /// <summary>Adds a custom tenant provider that obtains the user for each request.</summary>
        /// /// <returns>The current <see cref="MultitenantPermissionServerBuilder{T,K}"/> instance.</returns>
        public MultitenantPermissionServerBuilder<TPerm, TPermCat> AddTenantProvider<TProvider>()
            where TProvider : class, ITenantProvider
        {
            RemovePossibleDefault<TProvider>();
            Services.AddScoped<ITenantProvider, TProvider>();
            return this;
        }
    }
}