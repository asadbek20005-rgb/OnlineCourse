namespace OnlineCourse.Application.Dtos;

public class LessonDto
{
    public int CourseId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? VideoUrl { get; set; }

}
