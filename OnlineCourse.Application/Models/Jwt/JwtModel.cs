namespace OnlineCourse.Application.Models.Jwt;

public class JwtModel
{
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string SecurityKey { get; set; } = string.Empty;
}
