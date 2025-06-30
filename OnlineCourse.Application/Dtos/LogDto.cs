namespace OnlineCourse.Application.Dtos;

public class LogDto
{
    public Guid UserID { get; set; }
    public int Action { get; set; }
    public string IpAddress { get; set; } = string.Empty;
}
