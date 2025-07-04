using System.ComponentModel.DataAnnotations;

namespace OnlineCourse.Application.Models.Auth;

public class ResetPasswordModel
{
    [Required]
    public string Email { get; set; } = null!;

    [Required]
    public string Token { get; set; } = null!;

    [Required]
    public string NewPassword { get; set; } = null!;
}