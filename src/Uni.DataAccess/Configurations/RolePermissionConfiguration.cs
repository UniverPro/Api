using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Uni.DataAccess.Models;

namespace Uni.DataAccess.Configurations
{
    [UsedImplicitly]
    public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.ToTable(nameof(RolePermission));

            builder.HasKey(r => new { r.RoleId, r.PermissionId });
            
            builder.HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(ur => ur.RoleId);

            builder.HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(ur => ur.PermissionId);
        }
    }
}