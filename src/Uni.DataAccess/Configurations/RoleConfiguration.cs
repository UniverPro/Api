using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Uni.Core;
using Uni.DataAccess.Models;

namespace Uni.DataAccess.Configurations
{
    [UsedImplicitly]
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable(nameof(Role));

            builder.HasIndex(e => e.Name)
                .IsUnique();

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(Consts.MaxRoleNameLength);
        }
    }
}