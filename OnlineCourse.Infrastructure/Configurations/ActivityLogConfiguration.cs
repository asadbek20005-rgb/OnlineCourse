using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineCourse.Domain.Entities;

namespace OnlineCourse.Infrastructure.Configurations;

public class ActivityLogConfiguration : IEntityTypeConfiguration<ActivityLog>
{
    public void Configure(EntityTypeBuilder<ActivityLog> builder)
    {
        builder.ToTable("activity_logs");

        builder.HasKey(al => al.Id);
        builder.Property(al => al.Id).HasColumnName("id");

        builder.Property(al => al.UserID)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(al => al.TargetTable)
            .HasColumnName("target_table")
            .HasConversion<string>()
            .IsRequired();

        builder.Property(al => al.TargetTableId)
            .HasColumnName("target_table_id")
            .IsRequired();

        builder.Property(al => al.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(al => al.UpdatedAt)
            .HasColumnName("updated_at");

        builder.HasOne(al => al.User)
            .WithMany()
            .HasForeignKey(al => al.UserID)
            .OnDelete(DeleteBehavior.Cascade);
    }
}