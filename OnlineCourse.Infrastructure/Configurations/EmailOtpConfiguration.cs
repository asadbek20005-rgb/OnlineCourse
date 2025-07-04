using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineCourse.Domain.Entities;

namespace OnlineCourse.Infrastructure.Configurations;

public class EmailOtpConfiguration : IEntityTypeConfiguration<EmailOtp>
{
    public void Configure(EntityTypeBuilder<EmailOtp> builder)
    {
        builder.ToTable("otps");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
               .HasColumnName("id")
               .ValueGeneratedOnAdd();

        builder.Property(e => e.Email)
               .HasColumnName("email")
               .HasMaxLength(255)
               .IsRequired();

        builder.Property(e => e.Code)
               .HasColumnName("code")
               .IsRequired();

        builder.Property(e => e.IsExpired)
               .HasColumnName("is_expired")
               .HasDefaultValue(false);
    }
}
