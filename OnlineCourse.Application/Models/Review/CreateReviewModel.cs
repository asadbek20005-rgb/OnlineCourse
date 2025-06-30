using System.ComponentModel.DataAnnotations;

namespace OnlineCourse.Application.Models.Review;

public class CreateReviewModel
{
    [Required]
    public Guid UserID { get; set; }
    [Required]
    public int CourseId { get; set; }
    [Required]
    [MaxLength(100)]
    [MinLength(1)]
    [RegularExpression("^[\\s\\S]{3,500}$\r\n")]
    public string Comment { get; set; } = string.Empty;
    [Range(1, 5)]
    public int Rating { get; set; }
}
