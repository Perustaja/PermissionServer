using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PermissionServer.Common.Entities;
using PermissionServer.Common.Exceptions;
using PermissionServer.Common.Repositories;
using PermissionServer.Common.Services;

namespace PermissionServer.Common.Configuration
{
    public class PermissionServerBuilder<TPerm, TPermCat>
        where TPerm : Enum
        where TPermCat : Enum
    {
        private IServiceCollection Services { get; }
        public PermissionServerBuilder(IServiceCollection services)
        {
            Services = services;
        }

        /// <summary>
        /// Adds Entity Framework to the application, with seeding of user-extendable permission and 
        /// permission category entities.
        /// <returns>The current <see cref="PermissionServerBuilder{T,K}"/> instance.</returns>
        /// </summary>
        public PermissionServerBuilder<TPerm, TPermCat> AddEntityFrameworkStores<TContext>()
            where TContext : DbContext
        {
            var context = Activator.CreateInstance<TContext>() as TContext;
            var unregisteredEntityTypes = new List<Type>();
            if (context.Model.FindEntityType(typeof(Permission<TPerm, TPermCat>)) == null)
                unregisteredEntityTypes.Add(typeof(Permission<TPerm, TPermCat>));
            if (context.Model.FindEntityType(typeof(PermissionCategory<TPerm, TPermCat>)) == null)
                unregisteredEntityTypes.Add(typeof(PermissionCategory<TPerm, TPermCat>));

            if (unregisteredEntityTypes.Count > 0)
                throw new EntitiesNotRegisteredException(unregisteredEntityTypes.ToArray());

            Services.AddDbContext<TContext>();
            Services.AddScoped<IPermissionRepository<TPerm, TPermCat>, PermissionRepository<TPerm, TPermCat>>();
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

        protected void RemovePossibleDefault<TInterface>()
        {
            var possibleDefault =
                Services.FirstOrDefault(d => d.ServiceType == typeof(IUserProvider));
            if (possibleDefault != default(ServiceDescriptor))
                Services.Remove(possibleDefault);
        }
    }
}