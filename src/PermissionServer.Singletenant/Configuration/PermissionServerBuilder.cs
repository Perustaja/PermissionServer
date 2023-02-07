using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PermissionServer.Common.Configuration;
using PermissionServer.Common.Repositories;
using PermissionServer.Common.Services;
using PermissionServer.Singletenant.Authorization;
using Ps.Protobuf;

namespace PermissionServer.Singletenant.Configuration
{
    public sealed class PermissionServerBuilder<TPerm, TPermCat>
        where TPerm : Enum
        where TPermCat : Enum
    {
        private IServiceCollection _services;

        public PermissionServerBuilder(IServiceCollection services)
        { 
            _services = services;
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

            _services.AddGrpc();
            _services.AddSingleton<IAuthoritySettings>(new AuthoritySettings(isAuthority));
            if (isAuthority)
            {
                _services.AddGrpcClient<GrpcPermissionAuthorize.GrpcPermissionAuthorizeBase>(o =>
                {
                    o.Address = new Uri(remoteAddress);
                });
            }
            else
            {
                _services.AddGrpcClient<GrpcPermissionAuthorize.GrpcPermissionAuthorizeClient>(o =>
                {
                    o.Address = new Uri(remoteAddress);
                });
            }

            return this;
        }

        /// <summary>
        /// Adds Entity Framework to the application, with seeding of user-extendable permission and 
        /// permission category entities.
        /// </summary>
        /// <returns>The current <see cref="PermissionServerBuilder{T,K}"/> instance.</returns>
        public PermissionServerBuilder<TPerm, TPermCat> AddEntityFrameworkStores<TContext>()
            where TContext : DbContext
        {
            _services.AddDbContext<TContext>();
            _services.AddScoped<IPermissionRepository<TContext, TPerm, TPermCat>, PermissionRepository<TContext, TPerm, TPermCat>>();
            return this;
        }

        /// <summary>Adds a custom user provider that obtains the user for each request.</summary>
        /// /// <returns>The current <see cref="PermissionServerBuilder{T,K}"/> instance.</returns>
        public PermissionServerBuilder<TPerm, TPermCat> AddUserProvider<TProvider>()
            where TProvider : class, IUserProvider
        {
            RemovePossibleDefault<TProvider>();
            _services.AddScoped<IUserProvider, TProvider>();
            return this;
        }

        /// <summary>Adds a custom evaluator that determines authorization decisions.</summary>
        /// /// <returns>The current <see cref="PermissionServerBuilder{T,K}"/> instance.</returns>
        public PermissionServerBuilder<TPerm, TPermCat> AddAuthorizationEvaluator<TEvaluator>()
            where TEvaluator : class, IAuthorizationEvaluator
        {
            _services.AddScoped<IAuthorizationEvaluator, TEvaluator>();
            return this;
        }

        private void RemovePossibleDefault<TInterface>()
        {
            var possibleDefault =
                _services.FirstOrDefault(d => d.ServiceType == typeof(IUserProvider));
            if (possibleDefault != default(ServiceDescriptor))
                _services.Remove(possibleDefault);
        }
    }
}