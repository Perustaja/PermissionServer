using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using PermissionServer.Configuration;
using PermissionServer.Entities;
using PermissionServer.EntityFramework.Configuration.Common;

namespace PermissionServer.EntityFramework
{
    public class PSDbContext<TUser, TRole, TPerm, TPermCat> : IdentityDbContext<TUser, TRole, Guid>
        where TUser : PSUser<TPerm, TPermCat>
        where TRole : PSRole<TPerm, TPermCat>
        where TPerm : Enum
        where TPermCat : Enum
    {
        private readonly IConfiguration _config;
        private readonly MultitenantGlobalRolesOptions<TPerm, TPermCat> _globalRoleOptions;
        private readonly MultitenantEntityOptions<TPerm, TPermCat> _customEntityOptions;
        private readonly IPermissionSeeder<TPerm, TPermCat, Permission<TPerm, TPermCat>> _permSeeder;
        private readonly IPermissionCategorySeeder<TPerm, TPermCat, PermissionCategory<TPerm, TPermCat>> _permCatSeeder;
        public PSDbContext(DbContextOptions<PSDbContext<TUser, TRole, TPerm, TPermCat>> options,
            IOptions<MultitenantGlobalRolesOptions<TPerm, TPermCat>> globalRoleOptions,
            IOptions<MultitenantEntityOptions<TPerm, TPermCat>> customRoleOptions,
            IPermissionSeeder<TPerm, TPermCat, Permission<TPerm, TPermCat>> permSeeder,
            IPermissionCategorySeeder<TPerm, TPermCat, PermissionCategory<TPerm, TPermCat>> permCatSeeder)
            : base(options)
        {
            _globalRoleOptions = globalRoleOptions.Value ?? throw new ArgumentNullException(nameof(globalRoleOptions));
            _customEntityOptions = customRoleOptions.Value ?? throw new ArgumentNullException(nameof(customRoleOptions));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SeedPermissionData(modelBuilder); // must be done before others
            RegisterEntities(modelBuilder);
            SeedRoles(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void RegisterEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TUser>();
            modelBuilder.Entity<TRole>();
            modelBuilder.Entity(_customEntityOptions.RolePermissionType);
            modelBuilder.Entity(_customEntityOptions.TenantType);
            modelBuilder.Entity(_customEntityOptions.UserTenantType);
            modelBuilder.Entity(_customEntityOptions.UserTenantRoleType);
        }

        private void SeedPermissionData(ModelBuilder modelBuilder)
        {
            // Permissions seeding must be done in this order to seed properly
            modelBuilder.Entity<PermissionCategory<TPerm, TPermCat>>()
                .HasData(_permCatSeeder.GetSeedPermissionCategories());
            modelBuilder.Entity<Permission<TPerm, TPermCat>>().HasData(_permSeeder.GetSeedPermissions());
        }

        private void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TRole>().HasData(_globalRoleOptions.Roles);
            modelBuilder.Entity(_customEntityOptions.RolePermissionType)
                .HasData(_globalRoleOptions.RolePermissions);
        }
    }
}