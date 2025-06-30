namespace OnlineCourse.Application.Dtos;

public class StudentProgressDto
{
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public int LessonId { get; set; }
    public float ProgressPercent { get; set; }
    public DateTime? CompletedAt { get; set; }
}