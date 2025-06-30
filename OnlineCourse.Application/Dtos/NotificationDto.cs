namespace OnlineCourse.Application.Dtos;

public class NotificationDto
{
    public Guid UserID { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; } = false;
}
