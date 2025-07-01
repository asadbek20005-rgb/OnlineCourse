using Microsoft.EntityFrameworkCore;
using OnlineCourse.Domain.Bases;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineCourse.Domain.Entities;
[Table("instructors")]
[Index(nameof(UserId), IsUnique = true)]
public class Instructor : Base
{
    [Column("user_id")]
    public Guid UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }

    [Column("bio")]
    public string? Bio { get; set; }

    [Column("expariance_year")]
    [Required]
    public int Experiance { get; set; }

    [Column("approved")]
    public bool ApprovedByAdmin { get; set; } = false;

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

}
