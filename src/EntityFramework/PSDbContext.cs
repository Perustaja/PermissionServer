using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PermissionServer.Configuration;
using PermissionServer.Entities;
using PermissionServer.EntityFramework.Bases;
using PermissionServer.EntityFramework.Configuration.Common;

namespace PermissionServer.EntityFramework
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

        public override void RegisterEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TUser>();
            modelBuilder.Entity<TRole>();
            modelBuilder.Entity(_entityOptions.RolePermissionType);
            modelBuilder.Entity(_entityOptions.UserRoleType);
        }

        public override void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TRole>().HasData(_globalRoleOptions.Roles);
            modelBuilder.Entity(_entityOptions.RolePermissionType)
                .HasData(_globalRoleOptions.RolePermissions);
        }
    }
}