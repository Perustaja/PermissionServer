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

        /// <summary>Adds Entity Framework with a default <see cref="Authorization.IAuthorizationEvaluator"/>.</summary>
        /// <param name="options">An action to configure the <see cref="EntityFrameworkOptions{T,K}"/>.</param>
        /// <returns>The current <see cref="PermissionServerBuilder{T,K}"/> instance.</returns>
        public PermissionServerBuilder<TPerm, TPermCat> AddEntityFramework(
            Action<EntityFrameworkOptions<TPerm, TPermCat>> options)
        {
            var efo = new EntityFrameworkOptions<TPerm, TPermCat>();
            options?.Invoke(efo);
            Services.Configure(options);

            Services.AddScoped<IUserProvider, TokenSubjectUserProvider<TPerm, TPermCat>>();
            Services.AddScoped<ITenantProvider, RouteDataTenantProvider<TPerm, TPermCat>>();

            var customEval = typeof(AuthorizationEvaluator<,,,>)
                .MakeGenericType(efo.TenantType, efo.UserTenantType, typeof(TPerm), typeof(TPermCat));

            Services.AddScoped(typeof(IAuthorizationEvaluator), customEval);
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

        /// <summary>Adds a custom evaluator that determines authorization decisions.</summary>
        /// <returns>The current <see cref="PermissionServerBuilder{T,K}"/> instance.</returns>
        public PermissionServerBuilder<TPerm, TPermCat> AddAuthorizationEvaluator<TEvaluator>()
            where TEvaluator : class, IAuthorizationEvaluator
        {
            var possibleDefault =
                Services.FirstOrDefault(d => d.ServiceType == typeof(IAuthorizationEvaluator));
            if (possibleDefault != default(ServiceDescriptor))
            {
                if (!Services.Remove(possibleDefault))
                    throw new Exception("Attempted to remove default evaluator but unable to find it.");
            }

            Services.AddScoped<IAuthorizationEvaluator, TEvaluator>();
            return this;
        }
    }
}