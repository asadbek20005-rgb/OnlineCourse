namespace OnlineCourse.Application.Dtos;

public class ReviewDto
{
    public Guid UserID { get; set; }
    public int CourseId { get; set; }
    public string Comment { get; set; } = string.Empty;
    public int Rating { get; set; }
}