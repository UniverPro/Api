using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Uni.Core;
using Uni.DataAccess.Models;

namespace Uni.DataAccess.Configurations
{
    [UsedImplicitly]
    public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
    {
        public void Configure(EntityTypeBuilder<Subject> builder)
        {
            builder.ToTable("Subject");

            builder.HasIndex(e => e.Name);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(Consts.MaxNameLength);

            builder.HasOne(d => d.Group)
                .WithMany(p => p.Subjects)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Subject_Group");
        }
    }
}