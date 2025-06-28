using OnlineCourse.Domain.Bases;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineCourse.Domain.Entities;
[Table("student_progress")]
public class StudentProgress : Base
{
    [Column("student_id")]
    public int StudentId { get; set; }
    [ForeignKey(nameof(StudentId))]
    public Student? Student { get; set; }
    [Column("course_id")]
    public int CourseId { get; set; }
    [ForeignKey(nameof(CourseId))]
    public Course? Course { get; set; }

    [Column("lesson_id")]
    public int LessonId { get; set; }
    [ForeignKey(nameof(LessonId))]
    public Lesson? Lesson { get; set; }

    [Column("progress_percent")]
    public float ProgressPercent { get; set; }

    [Column("completed_at")]
    public DateTime? CompletedAt { get; set; }

}
