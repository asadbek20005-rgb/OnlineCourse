using OnlineCourse.Domain.Bases;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineCourse.Domain.Entities;
[Table("students")]
public class Student : Base
{
    [Column("user_id")]
    public Guid UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public virtual User? User { get; set; }
    public virtual ICollection<Enrollment>? Enrollments { get; set; }
    public virtual ICollection<StudentProgress>? StudentProgresses { get; set; }
}