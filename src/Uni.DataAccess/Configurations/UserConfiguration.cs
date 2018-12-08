using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Uni.Core;
using Uni.DataAccess.Models;

namespace Uni.DataAccess.Configurations
{
    [UsedImplicitly]
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(nameof(User));

            builder.HasIndex(e => e.Login)
                .IsUnique();

            builder.Property(e => e.Login)
                .IsRequired()
                .HasMaxLength(Consts.MaxLoginLength);

            builder.Property(e => e.Password)
                .IsRequired();

            builder.HasOne(x => x.Person)
                .WithOne(x => x.User)
                .HasForeignKey<User>(x => x.PersonId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
