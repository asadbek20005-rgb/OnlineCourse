using System.ComponentModel.DataAnnotations;

namespace OnlineCourse.Application.Models.Comment;

public class CreateCommecntModel
{
    public int LessonId { get; set; }
    [Required]
    public Guid UserID { get; set; }
    [Required]
    [MinLength(3)]
    public string Text { get; set; } = string.Empty;

}
