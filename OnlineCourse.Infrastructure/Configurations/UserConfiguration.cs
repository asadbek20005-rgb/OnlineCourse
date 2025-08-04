using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineCourse.Domain.Entities;

namespace OnlineCourse.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).HasColumnName("id");

        builder.Property(u => u.FullName)
            .HasColumnName("full_name")
            .HasMaxLength(70)
            .IsRequired();

        builder.Property(u => u.Email)
            .HasColumnName("email")
            .IsRequired();

        builder.HasIndex(u => u.Email).IsUnique();

        builder.Property(u => u.UserName)
            .HasColumnName("username")
            .IsRequired();

        builder.Property(u => u.PasswordHash)
            .HasColumnName("password_hash")
            .IsRequired();

        builder.Property(u => u.Role)
            .HasColumnName("role")
            .IsRequired();

        builder.Property(u => u.Status)
            .HasColumnName("status")
            .IsRequired();

        builder.Property(u => u.ImgUrl)
            .HasColumnName("img_url");

        builder.Property(u => u.LastLogin)
            .HasColumnName("last_login");

        builder.Property(u => u.EmailConfirmed)
            .HasColumnName("email_confirmation")
            .HasDefaultValue(false);

        builder.Property(u => u.IsBlocked)
            .HasColumnName("is_blocked")
            .HasDefaultValue(false);

        builder.HasOne(u => u.Instructor)
            .WithOne(i => i.User)
            .HasForeignKey<Instructor>(i => i.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(u => u.Student)
            .WithOne(s => s.User)
            .HasForeignKey<Student>(s => s.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(u => u.Notification)
            .WithOne(n => n.User)
            .HasForeignKey(n => n.UserID)
            .OnDelete(DeleteBehavior.Restrict);

    }
}
