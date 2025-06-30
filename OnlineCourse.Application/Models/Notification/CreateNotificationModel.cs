using System.ComponentModel.DataAnnotations;

namespace OnlineCourse.Application.Models.Notification;

public class CreateNotificationModel
{
    [Required]
    public Guid UserID { get; set; }
    [Required]
    [MaxLength(50)]
    [MinLength(3)]
    [RegularExpression("^(?!\\s)[a-zA-Z0-9?-??-???\\s\\-:',.&()]{3,100}(?<!\\s)$\r\n")]
    public string Title { get; set; } = string.Empty;
    [Required]
    [MaxLength(50)]
    [MinLength(3)]
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; } = false;
}
