using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Uni.Api.Core;
using Uni.Api.DataAccess.Models;

namespace Uni.Api.DataAccess.Configurations
{
    [UsedImplicitly]
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.ToTable(nameof(Person));
            
            builder.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(Consts.MaxNameLength);

            builder.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(Consts.MaxNameLength);
            
            builder.Property(e => e.MiddleName)
                .HasMaxLength(Consts.MaxNameLength);
            
            builder.HasIndex(e => e.Email)
                .IsUnique();

            builder.Property(e => e.Email)
                .HasMaxLength(Consts.MaxEmailLength);
            /*
            builder.HasOne(p => p.User)
                .WithOne(a => a.Person)
                .HasForeignKey<User>(a => a.PersonId);*/
        }
    }
}
