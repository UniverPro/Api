using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Uni.DataAccess.Models;

namespace Uni.DataAccess.Configurations
{
    public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
            builder.HasOne(d => d.Faculty)
                .WithMany(p => p.Teachers)
                .HasForeignKey(d => d.FacultyId)
                .HasConstraintName("FK_Person_Faculty");
        }
    }
}