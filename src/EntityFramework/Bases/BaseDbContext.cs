using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PermissionServer.Entities.Bases;
using PermissionServer.EntityFramework.Configuration;

namespace PermissionServer.EntityFramework.Bases
{
    public abstract class BaseDbContext<TUser, TRole, TPermEntity, TPermCatEntity, TPerm, TPermCat> 
    : IdentityDbContext<TUser, TRole, Guid>
        where TUser : BaseUser<TPerm, TPermCat>
        where TRole : BaseRole<TPerm, TPermCat>
        where TPermEntity : BasePermission<TPerm, TPermCat>
        where TPermCatEntity : BasePermissionCategory<TPerm, TPermCat>
        where TPerm : Enum
        where TPermCat : Enum
    {
        private readonly IPermissionSeeder<TPerm, TPermCat, TPermEntity> _permSeeder;
        private readonly IPermissionCategorySeeder<TPerm, TPermCat, TPermCatEntity> _permCatSeeder;
        public BaseDbContext(DbContextOptions options,
            IPermissionSeeder<TPerm, TPermCat, TPermEntity> permSeeder,
            IPermissionCategorySeeder<TPerm, TPermCat, TPermCatEntity> permCatSeeder)
            : base(options)
        {
            _permSeeder = permSeeder ?? throw new ArgumentNullException(nameof(permSeeder));
            _permCatSeeder = permCatSeeder ?? throw new ArgumentNullException(nameof(permCatSeeder));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            RegisterAndConfigureEntities(modelBuilder);
            SeedPermissionData(modelBuilder);
            SeedRoles(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void SeedPermissionData(ModelBuilder modelBuilder)
        {
            // Permissions seeding must be done in this order to seed properly
            modelBuilder.Entity<TPermEntity>().HasData(_permCatSeeder.GetSeedPermissionCategories());
            modelBuilder.Entity<TPermCatEntity>().HasData(_permSeeder.GetSeedPermissions());
        }

        /// <summary>
        /// Responsible for adding entities into the model and applying any configuration via fluent API.
        /// </summary>
        public abstract void RegisterAndConfigureEntities(ModelBuilder modelBuilder);
        /// <summary>Responsible for seeding any configured global roles into the database.</summary>
        public abstract void SeedRoles(ModelBuilder modelBuilder);
    }
}