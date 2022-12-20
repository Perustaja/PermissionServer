using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PermissionServer.Configuration;
using PermissionServer.EntityFramework.Bases;
using PermissionServer.Entities.Multitenancy;
using PermissionServer.EntityFramework.Configuration.Common;

namespace PermissionServer.EntityFramework.Multitenancy
{
    public class PSDbContext<TUser, TRole, TPerm, TPermCat> 
    : BaseDbContext<TUser, TRole, Permission<TPerm, TPermCat>, PermissionCategory<TPerm, TPermCat>, TPerm, TPermCat>
        where TUser : PSUser<TPerm, TPermCat>
        where TRole : PSRole<TPerm, TPermCat>
        where TPerm : Enum
        where TPermCat : Enum
    {
        private readonly MultitenantGlobalRoleOptions<TPerm, TPermCat> _globalRoleOptions;
        private readonly MultitenantEntityOptions<TPerm, TPermCat> _entityOptions;
        public PSDbContext(DbContextOptions<PSDbContext<TUser, TRole, TPerm, TPermCat>> options,
            IOptions<MultitenantGlobalRoleOptions<TPerm, TPermCat>> globalRoleOptions,
            IOptions<MultitenantEntityOptions<TPerm, TPermCat>> entityOptions,
            IPermissionSeeder<TPerm, TPermCat, Permission<TPerm, TPermCat>> permSeeder,
            IPermissionCategorySeeder<TPerm, TPermCat, PermissionCategory<TPerm, TPermCat>> permCatSeeder)
            : base(options, permSeeder, permCatSeeder)
        {
            _globalRoleOptions = globalRoleOptions.Value;
            _entityOptions = entityOptions.Value;
        }

        /// <summary>Responsible for adding all required entities to the dbcontext.</summary>
        public override void RegisterEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TUser>();
            modelBuilder.Entity<TRole>();
            modelBuilder.Entity<Permission<TPerm, TPermCat>>();
            modelBuilder.Entity<PermissionCategory<TPerm, TPermCat>>();
            modelBuilder.Entity(_entityOptions.RolePermissionType);
            modelBuilder.Entity(_entityOptions.TenantType);
            modelBuilder.Entity(_entityOptions.UserTenantType);
            modelBuilder.Entity(_entityOptions.UserTenantRoleType);
        }

        public override void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TRole>().HasData(_globalRoleOptions.Roles);
            modelBuilder.Entity(_entityOptions.RolePermissionType)
                .HasData(_globalRoleOptions.RolePermissions);
        }
    }
}