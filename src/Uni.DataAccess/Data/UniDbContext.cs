using Microsoft.EntityFrameworkCore;
using Uni.DataAccess.Models;

namespace Uni.DataAccess.Data
{
    public class UniDbContext : DbContext
    {
        public UniDbContext()
        {
        }

        public UniDbContext(DbContextOptions<UniDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Faculty> Faculties { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Person> Persons { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Teacher> Teachers { get; set; }
        public virtual DbSet<Schedule> Schedules { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<University> Universities { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Faculty>(entity =>
            {
                entity.ToTable("Faculty");

                entity.HasIndex(e => e.Name);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ShortName).HasMaxLength(50);

                entity.HasOne(d => d.University)
                    .WithMany(p => p.Faculties)
                    .HasForeignKey(d => d.UniversityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Faculty_University");
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.ToTable("Group");

                entity.HasIndex(e => e.Name);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Faculty)
                    .WithMany(p => p.Group)
                    .HasForeignKey(d => d.FacultyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Group_Faculty");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("Person");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.MiddleName).HasMaxLength(50);
            });

            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.HasOne(d => d.Faculty)
                    .WithMany(p => p.Teachers)
                    .HasForeignKey(d => d.FacultyId)
                    .HasConstraintName("FK_Person_Faculty");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("FK_Person_Group");
            });

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.ToTable("Schedule");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Schedule)
                    .HasForeignKey<Schedule>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Schedule_Subject");
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.ToTable("Subject");

                entity.HasIndex(e => e.Name);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Subjects)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Subject_Group");

                entity.HasOne(d => d.Teacher)
                    .WithMany(p => p.Subjects)
                    .HasForeignKey(d => d.TeacherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Subject_Teacher");
            });

            modelBuilder.Entity<University>(entity =>
            {
                entity.ToTable("University");

                entity.HasIndex(e => e.Name)
                    .IsUnique();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ShortName)
                    .HasMaxLength(50);
            });
        }
    }
}
