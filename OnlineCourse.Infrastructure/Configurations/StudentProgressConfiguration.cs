using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineCourse.Domain.Entities;

namespace OnlineCourse.Infrastructure.Configurations;

public class StudentProgressConfiguration : IEntityTypeConfiguration<StudentProgress>
{
    public void Configure(EntityTypeBuilder<StudentProgress> builder)
    {
        builder.ToTable("student_progress");

        builder.HasKey(sp => sp.Id);
        builder.Property(sp => sp.Id).HasColumnName("id");

        builder.Property(sp => sp.StudentId)
            .HasColumnName("student_id")
            .IsRequired();

        builder.Property(sp => sp.CourseId)
            .HasColumnName("course_id")
            .IsRequired();

        builder.Property(sp => sp.LessonId)
            .HasColumnName("lesson_id")
            .IsRequired();

        builder.Property(sp => sp.ProgressPercent)
            .HasColumnName("progress_percent")
            .IsRequired();

        builder.Property(sp => sp.CompletedAt)
            .HasColumnName("completed_at");

        builder.Property(sp => sp.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(sp => sp.UpdatedAt)
            .HasColumnName("updated_at");

        builder.HasOne(sp => sp.Student)
            .WithMany(s => s.StudentProgresses)
            .HasForeignKey(sp => sp.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(sp => sp.Course)
            .WithMany(c => c.StudentProgresses)
            .HasForeignKey(sp => sp.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(sp => sp.Lesson)
            .WithMany(l => l.StudentProgresses)
            .HasForeignKey(sp => sp.LessonId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
