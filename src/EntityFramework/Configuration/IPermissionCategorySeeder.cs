using PermissionServer.Entities.Bases;

namespace PermissionServer.EntityFramework.Configuration
{
    /// <summary>
    /// Transforms underlying permission category enum members into entities to be added to the database.
    /// </summary>
    /// <typeparam name="TPermCatEntity">
    /// <see cref="PermissionServer.Entities.PermissionCategory{TPerm, TPermCat}"/> or
    /// <see cref="PermissionServer.Entities.Multitenancy.PermissionCategory{TPerm, TPermCat}"/>
    /// depending on which version you are using.
    /// </typeparam>
    public interface IPermissionCategorySeeder<TPerm, TPermCat, TPermCatEntity>
        where TPerm : Enum
        where TPermCat : Enum
        where TPermCatEntity : BasePermissionCategory<TPerm, TPermCat>
    {
        /// <returns>The permission categories which should be seeded during migrations.</returns>
        IEnumerable<TPermCatEntity> GetSeedPermissionCategories();
    }
}