namespace OnlineCourse.Application.Models.Instructor;

public class CreateInstructorModel
{
    public Guid UserId { get; set; }
    public string? Bio { get; set; }
    public int Experiance { get; set; }
}