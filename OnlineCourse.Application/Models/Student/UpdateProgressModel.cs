namespace OnlineCourse.Application.Models.Student;

public class UpdateProgressModel
{
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public int LessonId { get; set; }
    public float ProgressPercent { get; set; }
}
