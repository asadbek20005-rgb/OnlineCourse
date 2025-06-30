namespace OnlineCourse.Application.Dtos;

public class CommentDto
{
    public int LessonId { get; set; }
    public Guid UserID { get; set; }
    public string Text { get; set; } = string.Empty;
    public int? ParentCommentId { get; set; }
}