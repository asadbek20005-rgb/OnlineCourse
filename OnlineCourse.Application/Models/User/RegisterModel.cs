using System.ComponentModel.DataAnnotations;

namespace OnlineCourse.Application.Models.User;

public class RegisterModel
{
    [Required]
    [MaxLength(70)]
    [MinLength(3)]
    [RegularExpression("^[A-Z][a-z]+(?:\\s[A-Z][a-z]+)+$\r\n")]
    public string FullName { get; set; } = string.Empty;


    [Required, EmailAddress]
    [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$\r\n")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [RegularExpression("^(?![0-9])[a-zA-Z0-9](?!.*[_.]{2})[a-zA-Z0-9._]{2,19}$\r\n")]
    public string UserName { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    [Required]
    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; } = string.Empty; 
}