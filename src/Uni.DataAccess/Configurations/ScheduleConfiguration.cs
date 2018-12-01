using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Uni.DataAccess.Models;

namespace Uni.DataAccess.Configurations
{
    [UsedImplicitly]
    public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
    {
        public void Configure(EntityTypeBuilder<Schedule> builder)
        {
            builder.ToTable("Schedule");

            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.StartTime)
                .HasColumnType("datetime");

            builder.HasOne(d => d.Subject)
                .WithMany(p => p.Schedules)
                .HasForeignKey(d => d.SubjectId)
                .HasConstraintName("FK_Schedule_Subject");

            builder.HasOne(d => d.Teacher)
                .WithMany(p => p.Schedules)
                .HasForeignKey(d => d.TeacherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Subject_Teacher");
        }
    }
}
