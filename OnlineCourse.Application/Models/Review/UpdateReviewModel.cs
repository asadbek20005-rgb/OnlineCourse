using System.ComponentModel.DataAnnotations;

namespace OnlineCourse.Application.Models.Review;

public class UpdateReviewModel
{
    public Guid? UserID { get; set; }
    public int? CourseId { get; set; }
    [MaxLength(100)]
    [MinLength(1)]
    [RegularExpression("^[\\s\\S]{3,500}$\r\n")]
    public string? Comment { get; set; } = string.Empty;
    [Range(1, 5)]
    public int? Rating { get; set; }
}
