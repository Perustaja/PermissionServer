using Microsoft.EntityFrameworkCore;
using PermissionServer.Common.Internal;

namespace PermissionServer.Common
{
    public static class ModelBuilderExtensions
    {
        /// <summary>
        /// Adds the permission and permission category entities, with seeding, into the context's model.
        /// </summary>
        /// <typeparam name="TPerm">Your application's permission enum.</typeparam>
        /// <typeparam name="TPermCat">Your application's permission category enum.</typeparam>
        /// <returns></returns>
        public static void AddPermissionServer<TPerm, TPermCat>(this ModelBuilder builder)
            where TPerm : Enum
            where TPermCat : Enum
        {
            builder.Entity<PermissionCategory<TPerm, TPermCat>>()
                .HasData(PermissionCategorySeeder<TPerm, TPermCat>.GetSeedPermissionCategories());
            builder.Entity<Permission<TPerm, TPermCat>>()
                .HasData(PermissionSeeder<TPerm, TPermCat>.GetSeedPermissions());
        }
    }
}