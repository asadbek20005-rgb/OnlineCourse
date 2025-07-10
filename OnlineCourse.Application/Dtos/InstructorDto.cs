namespace OnlineCourse.Application.Dtos;

public class InstructorDto
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public string? Bio { get; set; }
    public int Experiance { get; set; }
    public bool ApprovedByAdmin { get; set; } = false;

}