using PermissionServer.Multitenancy.Entities;

namespace PermissionServer.Multitenancy.EntityFramework.Repositories
{
    public interface IRolePermissionRepository<TRolePerm, TPerm, TPermCat>
        where TRolePerm : PSRolePermission<TPerm, TPermCat>
        where TPerm : Enum
        where TPermCat : Enum
    {
        void Add(params TRolePerm[] rolePerms);
    }
}