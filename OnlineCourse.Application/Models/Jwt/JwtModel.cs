namespace OnlineCourse.Application.Models.Jwt;

public class JwtModel
{
    public string Issue { get; set; } = string.Empty;
    public string Audiance { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
}
