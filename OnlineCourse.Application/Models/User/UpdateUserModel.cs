using System.ComponentModel.DataAnnotations;

namespace OnlineCourse.Application.Models.User;

public class UpdateUserModel
{
    [MaxLength(70)]
    [MinLength(3)]
    //[RegularExpression("^[A-Z][a-z]+(?:\\s[A-Z][a-z]+)+$\r\n")]
    public string? FullName { get; set; } = string.Empty;

    //[RegularExpression("^(?![0-9])[a-zA-Z0-9](?!.*[_.]{2})[a-zA-Z0-9._]{2,19}$\r\n")]w
    public string UserName { get; set; } = string.Empty;
}