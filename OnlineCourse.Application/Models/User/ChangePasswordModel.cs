using System.ComponentModel.DataAnnotations;

namespace OnlineCourse.Application.Models.User;

public class ChangePasswordModel
{
    public string? Email { get; set; }
    public string? UserName { get; set; }

    [Required]
    public string NewPassword { get; set; } = string.Empty;

    [Required]
    [Compare(nameof(NewPassword))]
    public string ConfirmPassword { get; set; } = string.Empty;

}