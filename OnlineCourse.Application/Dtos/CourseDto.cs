namespace OnlineCourse.Application.Dtos;

public class CourseDto
{
    public int Id { get; set; }
    public int InstructorId { get; set; }
    public string Title { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public int LevelId { get; set; }
    public string? CoverImgUrl { get; set; }
    public decimal Price { get; set; }
    public bool IsPublished { get; set; } = false;
    public decimal Rating { get; set; }
}
