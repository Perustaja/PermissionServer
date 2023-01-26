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
        protected IServiceCollection Services { get; }
        public PermissionServerBuilder(IServiceCollection services)
        {
            Services = services;
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