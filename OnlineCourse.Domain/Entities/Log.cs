using OnlineCourse.Domain.Bases;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineCourse.Domain.Entities;
[Table("logs")]
public class Log : Base
{
    [Column("user_id")]
    [Required]
    public Guid UserID { get; set; }
    [ForeignKey(nameof(UserID))]
    public User? User { get; set; }

    [Column("action")]
    [Required]  
    public LogActionType Action { get; set; }

    [Column("id_address")]
    [Required]
    public string IpAddress { get; set; } = string.Empty;



}


public enum LogActionType { Login, Logout, CreateCourse, DeleteLesson}
