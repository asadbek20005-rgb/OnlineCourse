using System.ComponentModel.DataAnnotations;

namespace OnlineCourse.Application.Models.User;

public class LoginModel
{

    [EmailAddress]
    [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$\r\n")]
    public string? Email { get; set; }

    [RegularExpression("^(?![0-9])[a-zA-Z0-9](?!.*[_.]{2})[a-zA-Z0-9._]{2,19}$\r\n")]
    public string? UserName { get; set; }

    [Required]
    public string Password { get; set; } = string.Empty;
}
