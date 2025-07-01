using System.ComponentModel.DataAnnotations;

namespace OnlineCourse.Application.Models.Course;

public class CreateCourseModel
{
    [Required]
    public int InstructorId { get; set; }
    [Required]
    [MaxLength(50)]
    [MinLength(3)]
    [RegularExpression("^(?!\\s)[a-zA-Z0-9?-??-???\\s\\-:',.&()]{3,100}(?<!\\s)$\r\n")]
    public string Title { get; set; } = string.Empty;
    [Required]
    public int CategoryId { get; set; }
    [Required]
    public int LevelId { get; set; }
    [Required]
    public decimal Price { get; set; }
}
