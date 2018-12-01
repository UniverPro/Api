using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Uni.Core;
using Uni.DataAccess.Models;

namespace Uni.DataAccess.Configurations
{
    [UsedImplicitly]
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.ToTable("Person");

            builder.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(Consts.MaxNameLength);

            builder.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(Consts.MaxNameLength);

            builder.Property(e => e.MiddleName)
                .HasMaxLength(Consts.MaxNameLength);
        }
    }
}
