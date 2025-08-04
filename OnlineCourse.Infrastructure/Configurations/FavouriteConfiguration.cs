using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineCourse.Domain.Entities;

namespace OnlineCourse.Infrastructure.Configurations;

public class FavouriteConfiguration : IEntityTypeConfiguration<Favourite>
{
    public void Configure(EntityTypeBuilder<Favourite> builder)
    {
        builder.ToTable("favourites");

        builder.HasKey(f => f.Id);
        builder.Property(f => f.Id).HasColumnName("id");

        builder.Property(f => f.UserID)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(f => f.CourseId)
            .HasColumnName("course_id")
            .IsRequired();

        builder.Property(f => f.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(f => f.UpdatedAt)
            .HasColumnName("updated_at");

        builder.HasOne(f => f.User)
            .WithMany()
            .HasForeignKey(f => f.UserID);
        builder.HasOne(f => f.Course)
            .WithMany(c => c.Favourites)
            .HasForeignKey(f => f.CourseId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasIndex(f => new { f.UserID, f.CourseId }).IsUnique();
    }
}