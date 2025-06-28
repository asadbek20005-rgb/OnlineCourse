using OnlineCourse.Domain.Bases;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineCourse.Domain.Entities;
[Table("comments")]
public class Comment : Base
{
    [Column("lesson_id")]
    public int LessonId { get; set; }
    [ForeignKey(nameof(LessonId))]
    public Lesson? Lesson { get; set; }


    [Column("user_id")]
    [Required]
    public Guid UserID { get; set; }
    [ForeignKey(nameof(UserID))]
    public User? User { get; set; }


    [Column("text")]
    [Required]
    [MinLength(3)]
    public string Text { get; set; } = string.Empty;



    [Column("parent_comment_id")]
    public int? ParentCommentId { get; set; }

    [ForeignKey(nameof(ParentCommentId))]
    public Comment? ParentComment { get; set; }

    public ICollection<Comment>? Replies { get; set; } 

}
