using System.ComponentModel.DataAnnotations;

namespace OnlineCourse.Application.Models.Blog;

public class CreateBlogModel
{
    [MaxLength(100)]
    [MinLength(3)]
    [Required]
    public string Title { get; set; } = string.Empty;

    [MaxLength(250)]
    [MinLength(10)]
    public string? Details { get; set; }

    public string? ImgUrl { get; set; }
    [Required]
    public Guid UserId { get; set; }
}
