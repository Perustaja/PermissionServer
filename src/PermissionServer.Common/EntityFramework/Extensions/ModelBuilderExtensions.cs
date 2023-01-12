using Microsoft.EntityFrameworkCore;
using PermissionServer.Common.Entities;

namespace PermissionServer.Common.EntityFramework.Extensions
{
    public static class ModelBuilderExtensions
    {
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