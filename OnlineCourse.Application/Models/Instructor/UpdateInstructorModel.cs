namespace OnlineCourse.Application.Models.Instructor;

public class UpdateInstructorModel
{
    public Guid? UserId { get; set; }
    public string? Bio { get; set; }
    public int? Experiance { get; set; }
    public bool ApprovedByAdmin { get; set; } = false;
}