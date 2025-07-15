using System.ComponentModel.DataAnnotations;

namespace OnlineCourse.Application.Models.Blog;

public class UpdateBlogModel
{
    [MaxLength(100)]
    [MinLength(3)]
    public string? Title { get; set; } = string.Empty;

    [MaxLength(250)]
    [MinLength(10)]
    public string? Details { get; set; }

    public string? ImgUrl { get; set; }

    public Guid? UserId { get; set; }
}
