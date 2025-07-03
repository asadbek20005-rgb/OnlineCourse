using OnlineCourse.Domain.Bases;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineCourse.Domain.Entities;
[Table("notifications")]
public class Notification : Base
{
    [Column("user_id")]
    [Required]
    public Guid UserID { get; set; }
    [ForeignKey(nameof(UserID))]
    public virtual User? User { get; set; }

    [Column("title")]
    [Required]
    [MaxLength(50)]
    [MinLength(3)]
    [RegularExpression("^(?!\\s)[a-zA-Z0-9?-??-???\\s\\-:',.&()]{3,100}(?<!\\s)$\r\n")]
    public string Title { get; set; } = string.Empty;


    [Column("message")]
    [Required]
    [MaxLength(50)]
    [MinLength(3)]
    public string Message { get; set; } = string.Empty;


    [Column("is_read")]
    public bool IsRead { get; set; } = false;


}
