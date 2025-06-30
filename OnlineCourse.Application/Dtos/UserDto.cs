namespace OnlineCourse.Application.Dtos;

public class UserDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public int Role { get; set; }
    public int Status { get; set; }
    public string? ImgUrl { get; set; }
    public DateTime? LastLogin { get; set; }
    public bool EmailConfirmed { get; set; } = false;
}

