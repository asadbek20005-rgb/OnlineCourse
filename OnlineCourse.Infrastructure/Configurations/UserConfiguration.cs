using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineCourse.Domain.Entities;

namespace OnlineCourse.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(entity => new { entity.Id });

        builder.HasAlternateKey(entity => entity.Email);

        builder.Property(entity => entity.FullName)
            .IsRequired()
            .HasMaxLength(70);

        builder.HasIndex(entity => entity.Email)
            .IsUnique(true);


    }
}
