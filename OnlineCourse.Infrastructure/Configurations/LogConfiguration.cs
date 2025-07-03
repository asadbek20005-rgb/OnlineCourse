using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineCourse.Domain.Entities;

namespace OnlineCourse.Infrastructure.Configurations;

public class LogConfiguration : IEntityTypeConfiguration<Log>
{
    public void Configure(EntityTypeBuilder<Log> builder)
    {
        builder.ToTable("logs");

        builder.HasKey(l => l.Id);
        builder.Property(l => l.Id).HasColumnName("id");

        builder.Property(l => l.UserID)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(l => l.Action)
            .HasColumnName("action")
            .IsRequired();

        builder.Property(l => l.IpAddress)
            .HasColumnName("id_address")
            .IsRequired();

        builder.Property(l => l.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(l => l.UpdatedAt)
            .HasColumnName("updated_at");

        builder.HasOne(l => l.User)
            .WithMany()
            .HasForeignKey(l => l.UserID)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
