using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Uni.Core;
using Uni.DataAccess.Models;

namespace Uni.DataAccess.Configurations
{
    [UsedImplicitly]
    public class FacultyConfiguration : IEntityTypeConfiguration<Faculty>
    {
        public void Configure(EntityTypeBuilder<Faculty> builder)
        {
            builder.ToTable(nameof(Faculty));

            builder.HasIndex(e => e.Name);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(Consts.MaxNameLength);

            builder.Property(e => e.ShortName)
                .HasMaxLength(Consts.MaxShortNameLength);

            builder.HasOne(d => d.University)
                .WithMany(p => p.Faculties)
                .HasForeignKey(d => d.UniversityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Faculty_University");
        }
    }
}
