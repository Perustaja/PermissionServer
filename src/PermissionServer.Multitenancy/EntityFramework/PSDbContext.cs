using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PermissionServer.Common.EntityFramework;
using PermissionServer.Multitenancy.Configuration;
using PermissionServer.Multitenancy.Entities;

namespace PermissionServer.Multitenancy.EntityFramework
{
    public class PSDbContext<TUser, TRole, TPerm, TPermCat>
    : BaseDbContext<TUser, TRole, Permission<TPerm, TPermCat>, PermissionCategory<TPerm, TPermCat>, TPerm, TPermCat>
        where TUser : PSUser<TPerm, TPermCat>
        where TRole : PSRole<TPerm, TPermCat>
        where TPerm : Enum
        where TPermCat : Enum
    {
        private readonly GlobalRoleOptions<TPerm, TPermCat> _globalRoleOptions;
        private readonly EntityOptions<TPerm, TPermCat> _entityOptions;
        public PSDbContext(DbContextOptions<PSDbContext<TUser, TRole, TPerm, TPermCat>> options,
            IOptions<GlobalRoleOptions<TPerm, TPermCat>> globalRoleOptions,
            IOptions<EntityOptions<TPerm, TPermCat>> entityOptions,
            IPermissionSeeder<TPerm, TPermCat, Permission<TPerm, TPermCat>> permSeeder,
            IPermissionCategorySeeder<TPerm, TPermCat, PermissionCategory<TPerm, TPermCat>> permCatSeeder)
            : base(options, permSeeder, permCatSeeder)
        {
            _globalRoleOptions = globalRoleOptions.Value;
            _entityOptions = entityOptions.Value;
        }

        /// <summary>
        /// Responsible for adding entities into the model and applying any configuration via fluent API.
        /// </summary>
        public override void RegisterAndConfigureEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TUser>();
            modelBuilder.Entity<TRole>();
            modelBuilder.Entity<Permission<TPerm, TPermCat>>();
            modelBuilder.Entity<PermissionCategory<TPerm, TPermCat>>();
            modelBuilder.Entity(_entityOptions.RolePermissionType);
            modelBuilder.Entity(_entityOptions.TenantType);
            modelBuilder.Entity(_entityOptions.UserTenantType);
            modelBuilder.Entity(_entityOptions.UserTenantRoleType);

            ConfigureJoinTables(modelBuilder);
        }

        /// <summary>Responsible for seeding any configured global roles into the database.</summary>
        public override void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TRole>().HasData(_globalRoleOptions.Roles);
            modelBuilder.Entity(_entityOptions.RolePermissionType)
                .HasData(_globalRoleOptions.RolePermissions);
        }

        private void ConfigureJoinTables(ModelBuilder modelBuilder)
        {
            // many-many relationships only work without explicit fluent api configuration if
            // the join table can be generated without extra data. because extra data is desired for 
            // user-tenant, and it should be possible for end users to add their own is desired, fluent
            // api is done for all many-many table configs
            // https://learn.microsoft.com/en-us/ef/core/modeling/relationships?tabs=fluent-api%2Cfluent-api-simple-key%2Csimple-key#join-entity-type-configuration

            // rolepermission
            modelBuilder.Entity(_entityOptions.RolePermissionType).HasKey("RoleId", "PermissionId");
            modelBuilder.Entity(_entityOptions.RolePermissionType)
                .HasOne("Role")
                .WithMany("RolePermissions")
                .HasForeignKey("RoleId");
            modelBuilder.Entity(_entityOptions.RolePermissionType)
                .HasOne("Permission")
                .WithMany("RolePermissions")
                .HasForeignKey("PermissionId");

            // usertenant
            modelBuilder.Entity(_entityOptions.UserTenantType).HasKey("UserId", "TenantId");
            modelBuilder.Entity(_entityOptions.UserTenantType)
                .HasOne("User")
                .WithMany("UserTenants")
                .HasForeignKey("UserId");
            modelBuilder.Entity(_entityOptions.UserTenantType)
                .HasOne("Tenant")
                .WithMany("UserTenants")
                .HasForeignKey("TenantId");

            // usertenantrole
            modelBuilder.Entity(_entityOptions.UserTenantRoleType)
                .HasKey("UserId", "TenantId", "RoleId");
            modelBuilder.Entity(_entityOptions.UserTenantRoleType)
                .HasOne("User")
                .WithMany("UserTenantRoles")
                .HasForeignKey("UserId");
            modelBuilder.Entity(_entityOptions.UserTenantRoleType)
                .HasOne("Tenant")
                .WithMany("UserTenantRoles")
                .HasForeignKey("TenantId");
            modelBuilder.Entity(_entityOptions.UserTenantRoleType)
                .HasOne("Role")
                .WithMany("UserTenantRoles")
                .HasForeignKey("RoleId");
        }
    }
}