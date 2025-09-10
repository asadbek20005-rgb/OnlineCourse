namespace OnlineCourse.Application.Dtos;

public record CourseDto(
    int Id,
    int InstructorId,
    string Title,
    int CategoryId,
    int LevelId,
    string? CoverImgUrl,
    decimal Price,
    bool IsPublished,
    decimal Rating
);
