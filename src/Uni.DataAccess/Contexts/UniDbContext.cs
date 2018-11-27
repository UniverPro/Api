using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Uni.DataAccess.Configurations;
using Uni.DataAccess.Models;

namespace Uni.DataAccess.Contexts
{
    public class UniDbContext : DbContext
    {
        [SuppressMessage("ReSharper", "SuggestBaseTypeForParameter", Justification = "This should be strong-typed")]
        public UniDbContext(DbContextOptions<UniDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Faculty> Faculties { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Teacher> Teachers { get; set; }
        public virtual DbSet<Schedule> Schedules { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<University> Universities { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FacultyConfiguration());
            modelBuilder.ApplyConfiguration(new GroupConfiguration());
            modelBuilder.ApplyConfiguration(new PersonConfiguration());
            modelBuilder.ApplyConfiguration(new ScheduleConfiguration());
            modelBuilder.ApplyConfiguration(new StudentConfiguration());
            modelBuilder.ApplyConfiguration(new SubjectConfiguration());
            modelBuilder.ApplyConfiguration(new TeacherConfiguration());
            modelBuilder.ApplyConfiguration(new UniversityConfiguration());
        }
    }
}
