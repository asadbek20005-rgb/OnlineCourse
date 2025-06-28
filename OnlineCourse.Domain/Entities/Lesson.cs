using OnlineCourse.Domain.Bases;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineCourse.Domain.Entities;

[Table("lesson")]
public class Lesson : Base
{
    [Column("course_id")]
    public int CourseId { get; set; }

    [ForeignKey(nameof(CourseId))]
    public Course? Course { get; set; }


    [Column("title")]
    [Required]
    [MaxLength(50)]
    [MinLength(3)]
    [RegularExpression("^(?!\\s)[a-zA-Z0-9?-??-???\\s\\-:',.&()]{3,100}(?<!\\s)$\r\n")]
    public string Title { get; set; } = string.Empty;


    [Column("video_url")]
    public string? VideoUrl { get; set; }

    public ICollection<Comment>? Comments { get; set; }

}
