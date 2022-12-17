using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PermissionServer.Entities.Multitenancy;

namespace PermissionServer.EntityFramework.Configuration.Multitenancy
{
    public class UserTenantRoleConfiguration<TPerm, TPermCat> :
    IEntityTypeConfiguration<PSUserTenantRole<TPerm, TPermCat>>
        where TPerm : Enum
        where TPermCat : Enum
    {
        public void Configure(EntityTypeBuilder<PSUserTenantRole<TPerm, TPermCat>> builder)
        {
            builder.HasKey(uor => new { uor.UserId, uor.TenantId, uor.RoleId });
            builder
                .HasOne(uor => uor.User)
                .WithMany(u => u.UserTenantRoles)
                .HasForeignKey(uor => uor.UserId);
            builder
                .HasOne(uor => uor.Tenant)
                .WithMany(o => o.UserTenantRoles)
                .HasForeignKey(uor => uor.TenantId);
            builder
                .HasOne(uor => uor.Role)
                .WithMany(r => r.UserTenantRoles)
                .HasForeignKey(uor => uor.RoleId);
        }
    }
}