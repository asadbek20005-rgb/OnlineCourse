namespace OnlineCourse.Application.Models.User;

public record ChangePasswordModel(
    string? Email,
    string? UserName,
    string NewPassword,
    string ConfirmPassword
);