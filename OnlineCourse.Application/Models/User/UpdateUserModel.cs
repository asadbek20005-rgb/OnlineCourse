namespace OnlineCourse.Application.Models.User;

public record UpdateUserModel(
    string? FullName = "",
    string UserName = ""
);
