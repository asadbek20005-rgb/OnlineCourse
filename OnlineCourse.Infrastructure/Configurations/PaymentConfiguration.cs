using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineCourse.Domain.Entities;

namespace OnlineCourse.Infrastructure.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("payments");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasColumnName("id");

        builder.Property(p => p.UserID)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(p => p.Amount)
            .HasColumnName("amount")
            .IsRequired();

        builder.Property(p => p.CourseId)
            .HasColumnName("course_id")
            .IsRequired();

        builder.Property(p => p.IsVerified)
            .HasColumnName("is_verified")
            .HasDefaultValue(false);

        builder.Property(p => p.PaymentDate)
            .HasColumnName("payment_date")
            .IsRequired();

        builder.Property(p => p.HasPaid)
            .HasColumnName("has_paid")
            .HasDefaultValue(false);

        builder.Property(p => p.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(p => p.UpdatedAt)
            .HasColumnName("updated_at");

        builder.HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserID)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.Course)
            .WithMany(c => c.Payments)
            .HasForeignKey(p => p.CourseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
