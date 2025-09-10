using System.ComponentModel.DataAnnotations;

namespace OnlineCourse.Application.Models.Blog;

public record CreateBlogModel(
    [property: MaxLength(100), MinLength(3), Required] string Title,
    [property: MaxLength(250), MinLength(10)] string? Details,
    string? ImgUrl,
    [property: Required] Guid UserId
);
