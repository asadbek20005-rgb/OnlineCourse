using System.ComponentModel.DataAnnotations;

namespace OnlineCourse.Application.Models.Course;

public record CreateCourseModel(
    [property: Required] int InstructorId,
    [property: Required, MaxLength(50), MinLength(3)] string Title,
    [property: Required] int CategoryId,
    [property: Required] int LevelId,
    [property: Required] decimal Price
);