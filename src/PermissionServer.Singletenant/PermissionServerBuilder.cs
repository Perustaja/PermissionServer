using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PermissionServer.Common;
using PermissionServer.Common.Internal;
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

        /// <summary>Adds remote authorization to the application with this application as a client.</summary>
        /// <param name="remoteAddress">The url of the authority server, e.g. "https://localhost:5000".</param>
        /// <returns>The current <see cref="PermissionServerBuilder{T,K}"/> instance.</returns>
        public PermissionServerBuilder<TPerm, TPermCat> AddRemoteAuthorization(string remoteAddress)
        {
            if (String.IsNullOrEmpty(remoteAddress))
                throw new ArgumentNullException("Remote address cannot be null or empty.");

            _services.AddGrpc();
            _services.AddSingleton<IAuthoritySettings>(new AuthoritySettings(false));
            _services.AddGrpcClient<GrpcPermissionAuthorize.GrpcPermissionAuthorizeClient>(o =>
            {
                o.Address = new Uri(remoteAddress);
            });

            return this;
        }

        /// <summary>Adds remote authorization to the application, with this server acting as the authority.</summary>
        /// <param name="remoteAddress">The url of the remote client, e.g. "https://localhost:5000"</param>
        /// <typeparam name="TEvaluator">Your implementation of <see cref="IAuthorizationEvaluator"/>.</typeparam>
        /// <returns>The current <see cref="PermissionServerBuilder{T,K}"/> instance.</returns>
        public PermissionServerBuilder<TPerm, TPermCat> AddRemoteAuthorization<TEvaluator>(
            string remoteAddress)
            where TEvaluator : class, IAuthorizationEvaluator
        {
            if (String.IsNullOrEmpty(remoteAddress))
                throw new ArgumentNullException("Remote address cannot be null or empty.");

            _services.AddGrpc();
            _services.AddSingleton<IAuthoritySettings>(new AuthoritySettings(true));
            _services.AddScoped<IAuthorizationEvaluator, TEvaluator>();
            _services.AddGrpcClient<GrpcPermissionAuthorize.GrpcPermissionAuthorizeBase>(o =>
            {
                o.Address = new Uri(remoteAddress);
            });

            return this;
        }

        /// <summary>
        /// Adds remote authorization to the application, with this server acting as a client with 
        /// multiple authorities.
        /// </summary>
        /// <param name="servers">
        /// Tuples representing each server this client must call. Each has a title string and a Uri
        /// for the address of the server. The title you enter is what you will use in your remote
        /// authorization attributes.<br/> e.g. ("IdentityServer", new Uri("localhost:5000")).
        /// </param>
        /// <returns>The current <see cref="PermissionServerBuilder{T,K}"/> instance.</returns>
        public PermissionServerBuilder<TPerm, TPermCat> AddRemoteAuthorization(
            params (string, Uri)[] servers)
        {
            if (servers.GroupBy(vt => vt.Item1).Any(g => g.Count() > 1))
                throw new ArgumentException("Server titles must be unique. Found a duplicate title in authorities registered.");

            _services.AddGrpc();
            _services.AddSingleton<IAuthoritySettings>(new AuthoritySettings(false));
            foreach (var s in servers)
            {
                _services.AddGrpcClient<GrpcPermissionAuthorize.GrpcPermissionAuthorizeClient>(s.Item1, o =>
                {
                    o.Address = s.Item2;
                });
            }

            return this;
        }

        /// <summary>
        /// Adds remote authorization to the application, with this server acting as the authority
        /// for multiple clients.</summary>
        /// <param name="remoteAddresses">The urls of the remote clients, e.g. "https://localhost:5000"</param>
        /// <typeparam name="TEvaluator">Your implementation of <see cref="IAuthorizationEvaluator"/>.</typeparam>
        /// <returns>The current <see cref="PermissionServerBuilder{T,K}"/> instance.</returns>
        public PermissionServerBuilder<TPerm, TPermCat> AddRemoteAuthorization<TEvaluator>(
            string[] remoteAddresses)
            where TEvaluator : class, IAuthorizationEvaluator
        {
            if (remoteAddresses.Count() < 1 || remoteAddresses.Any(a => String.IsNullOrEmpty(a)))
                throw new ArgumentNullException("Remote address array cannot be empty or contain empty strings.");

            _services.AddGrpc();
            _services.AddSingleton<IAuthoritySettings>(new AuthoritySettings(true));
            _services.AddScoped<IAuthorizationEvaluator, TEvaluator>();
            foreach (string a in remoteAddresses)
            {
                _services.AddGrpcClient<GrpcPermissionAuthorize.GrpcPermissionAuthorizeBase>(o =>
                {
                    o.Address = new Uri(a);
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