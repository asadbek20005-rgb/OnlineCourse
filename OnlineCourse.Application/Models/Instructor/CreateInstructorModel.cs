namespace OnlineCourse.Application.Models.Instructor;

public record CreateInstructorModel(
    Guid UserId,
    string? Bio,
    int Experiance
);
