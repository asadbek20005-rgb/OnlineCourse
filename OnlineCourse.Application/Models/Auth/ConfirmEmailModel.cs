using System.ComponentModel.DataAnnotations;

namespace OnlineCourse.Application.Models.Auth;

public class ConfirmEmailModel
{
    [Required]
    public string Email { get; set; }
    [Required]
    public int Code { get; set; }
}
