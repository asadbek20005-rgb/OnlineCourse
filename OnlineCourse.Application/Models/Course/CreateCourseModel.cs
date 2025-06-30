using System.ComponentModel.DataAnnotations;

namespace OnlineCourse.Application.Models.Course;

public class CreateCourseModel
{
    public int InstructorId { get; set; }
    [Required]
    [MaxLength(50)]
    [MinLength(3)]
    [RegularExpression("^(?!\\s)[a-zA-Z0-9?-??-???\\s\\-:',.&()]{3,100}(?<!\\s)$\r\n")]
    public string Title { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public int LevelId { get; set; }
    [Required]
    public decimal Price { get; set; }
    public bool IsPublished { get; set; } = false;
    public decimal Rating { get; set; }
}
