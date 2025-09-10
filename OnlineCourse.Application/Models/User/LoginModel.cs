namespace OnlineCourse.Application.Models.User;


public record LoginModel(
    string? Email,
    string? UserName,
    string Password = ""
);
