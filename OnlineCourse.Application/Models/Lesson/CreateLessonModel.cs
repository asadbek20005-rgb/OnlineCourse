using System.ComponentModel.DataAnnotations;

namespace OnlineCourse.Application.Models.Lesson;

public class CreateLessonModel
{
    public int CourseId { get; set; }

    [Required]
    [MaxLength(50)]
    [MinLength(3)]
    [RegularExpression("^(?!\\s)[a-zA-Z0-9?-??-???\\s\\-:',.&()]{3,100}(?<!\\s)$\r\n")]
    public string Title { get; set; } = string.Empty;
}