using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Uni.DataAccess.Models;

namespace Uni.DataAccess.Configurations
{
    [UsedImplicitly]
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable(nameof(UserRole));

            builder.HasKey(r => new { r.UserId, r.RoleId });

            builder.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            builder.HasOne(ur => ur.User)
                .WithMany(p => p.UserRoles)
                .HasForeignKey(ur => ur.UserId);
        }
    }
}