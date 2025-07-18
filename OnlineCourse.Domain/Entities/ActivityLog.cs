using OnlineCourse.Domain.Bases;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineCourse.Domain.Entities;
[Table("activity_logs")]
public class ActivityLog : Base
{
    [Column("user_id")]
    [Required]
    public Guid UserID { get; set; }
    [ForeignKey(nameof(UserID))]
    public virtual User? User { get; set; }
    [Column("target_table")]
    [Required]
    public ActivityTargetType TargetTable { get; set; }

    [Column("target_table_id")]
    [Required]
    public string TargetTableId { get; set; } = string.Empty;

}

public enum ActivityTargetType
{
    User,
    StudentProgress,
    Student,
    Review,
    RefreshToken,
    Payment,
    Notification,
    Log,
    Level,
    Lesson,
    Instructor,
    Favourite,
    Enrollment,
    Course,
    Comment,
    Category,
}