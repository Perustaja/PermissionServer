using PermissionServer.Multitenancy.Entities;

namespace PermissionServer.Multitenancy.Services
{
    public class DefaultRoleManager<TRole, TPerm, TPermCat> : IRoleManager<TRole, TPerm, TPermCat>
        where TRole : PSRole<TPerm, TPermCat>
        where TPerm : Enum
        where TPermCat : Enum
    {

    }
}