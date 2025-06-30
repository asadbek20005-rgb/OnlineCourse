namespace OnlineCourse.Application.Dtos;

public class ActivityLogDto
{
    public Guid UserID { get; set; }
    public int TargetTable { get; set; }
    public string TargetTableId { get; set; } = string.Empty;
}
