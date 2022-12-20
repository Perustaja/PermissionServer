using PermissionServer.Entities.Bases;

namespace PermissionServer.EntityFramework.Configuration
{
    /// <summary>
    /// Transforms underlying permission enum members into entities to be added to the database.
    /// </summary>
    /// <typeparam name="TPermEntity">
    /// <see cref="PermissionServer.Entities.Permission{TPerm, TPermCat}"/> or
    /// <see cref="PermissionServer.Entities.Multitenancy.Permission{TPerm, TPermCat}"/>
    /// depending on which version you are using.
    /// </typeparam>
    public interface IPermissionSeeder<TPerm, TPermCat, TPermEntity>
        where TPerm : Enum
        where TPermCat : Enum
        where TPermEntity : BasePermission<TPerm, TPermCat>
    {
        /// <returns>The permissions which should be seeded during migrations.</returns>
        IEnumerable<TPermEntity> GetSeedPermissions();
    }
}