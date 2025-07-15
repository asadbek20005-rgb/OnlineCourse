using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineCourse.Domain.Entities;

namespace OnlineCourse.Infrastructure.Configurations;

public class BlogConfiguration : IEntityTypeConfiguration<Blog>
{
    public void Configure(EntityTypeBuilder<Blog> entity)
    {
        entity.ToTable("blogs");

        entity.Property(e => e.Title)
            .HasColumnName("title")
            .HasMaxLength(100)
            .IsRequired();

        entity.Property(e => e.Details)
            .HasColumnName("details")
            .HasMaxLength(250);

        entity.Property(e => e.ImgUrl)
            .HasColumnName("img_url");

        entity.Property(e => e.UserId)
            .HasColumnName("user_id");

        entity.HasOne(e => e.User)
            .WithMany(u => u.Blogs)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasMany(e => e.Comments)
            .WithOne(c => c.Blog)
            .HasForeignKey(c => c.BlogId);
    }
}
