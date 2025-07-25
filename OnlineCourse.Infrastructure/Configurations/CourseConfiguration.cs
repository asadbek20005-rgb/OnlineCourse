using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Minio.DataModel.Notification;
using OnlineCourse.Domain.Entities;

namespace OnlineCourse.Infrastructure.Configurations;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.ToTable("courses");
        builder.Property(e => e.Price)
             .HasPrecision(18, 2); 

        builder.Property(e => e.Rating)
              .HasPrecision(5, 2);

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasColumnName("id");

        builder.Property(c => c.InstructorId)
            .HasColumnName("instructor_id")
            .IsRequired();

        builder.Property(c => c.Title)
            .HasColumnName("title")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.CategoryId)
            .HasColumnName("category_id")
            .IsRequired();

        builder.Property(c => c.LevelId)
            .HasColumnName("level_id")
            .IsRequired();

        builder.Property(c => c.CoverImgUrl)
            .HasColumnName("cover_img_url");

        builder.Property(c => c.Price)
            .HasColumnName("price")
            .IsRequired();

        builder.Property(c => c.IsPublished)
            .HasColumnName("is_published")
            .HasDefaultValue(false);

        builder.Property(c => c.Rating)
            .HasColumnName("rating");

        builder.Property(c => c.HasCompleted)
            .HasColumnName("has_completed")
            .HasDefaultValue(false);

        builder.Property(c => c.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(c => c.UpdatedAt)
            .HasColumnName("updated_at");

        builder.HasOne(c => c.Instructor)
            .WithMany(i => i.Courses)
            .HasForeignKey(c => c.InstructorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.Category)
            .WithMany()
            .HasForeignKey(c => c.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.Level)
            .WithMany()
            .HasForeignKey(c => c.LevelId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Lessons)
            .WithOne(l => l.Course)
            .HasForeignKey(l => l.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Reviews)
            .WithOne(r => r.Course)
            .HasForeignKey(r => r.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Favourites)
            .WithOne(f => f.Course)
            .HasForeignKey(f => f.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.StudentProgresses)
            .WithOne(sp => sp.Course)
            .HasForeignKey(sp => sp.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Payments)
            .WithOne(p => p.Course)
            .HasForeignKey(p => p.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Enrollments)
            .WithOne(e => e.Course)
            .HasForeignKey(e => e.CourseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
