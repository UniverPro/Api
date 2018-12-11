using Microsoft.EntityFrameworkCore;
using Uni.Api.Core.Extensions;
using Uni.Api.DataAccess.Configurations;
using Uni.Api.DataAccess.Models;

namespace Uni.Api.DataAccess.Contexts
{
    public class UniDbContext : DbContext
    {
        public UniDbContext(DbContextOptions<UniDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<UserRole> UserRoles { get; set; }

        public virtual DbSet<Role> Roles { get; set; }

        public virtual DbSet<RolePermission> RolePermissions { get; set; }

        public virtual DbSet<Permission> Permissions { get; set; }

        public virtual DbSet<Faculty> Faculties { get; set; }

        public virtual DbSet<Group> Groups { get; set; }

        public virtual DbSet<Student> Students { get; set; }

        public virtual DbSet<Teacher> Teachers { get; set; }

        public virtual DbSet<Schedule> Schedules { get; set; }

        public virtual DbSet<Subject> Subjects { get; set; }

        public virtual DbSet<University> Universities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyAllConfigurationsFromAssemblyContaining<EntityTypeConfigurationsMarker>();
        }
    }
}
