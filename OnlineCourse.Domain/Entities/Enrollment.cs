using OnlineCourse.Domain.Bases;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineCourse.Domain.Entities;
[Table("enrollements")]
public class Enrollment : Base
{
    [Column("student_id")]
    [Required]
    public int StudentId { get; set; }
    [ForeignKey(nameof(StudentId))]
    public virtual Student? Student { get; set; }


    [Column("course_id")]
    [Required]
    public int CourseId { get; set; }
    [ForeignKey(nameof(CourseId))]  
    public virtual Course? Course { get; set; }
    [Column("enrolled_at")]
    [Required]
    public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;

}