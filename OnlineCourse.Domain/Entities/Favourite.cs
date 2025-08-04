using OnlineCourse.Domain.Bases;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineCourse.Domain.Entities;
[Table("favourites")]
public class Favourite : Base
{
    [Column("user_id")]
    [Required]
    public Guid UserID { get; set; }
    //[ForeignKey(nameof(UserID))]
    public virtual User? User { get; set; }

    [Column("course_id")]
    [Required]
    public int CourseId { get; set; }
    [ForeignKey(nameof(CourseId))]
    public virtual Course? Course { get; set; }
}
