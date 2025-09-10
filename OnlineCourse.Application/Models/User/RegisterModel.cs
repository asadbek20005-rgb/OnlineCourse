namespace OnlineCourse.Application.Models.User;

public record RegisterModel(
    string FullName = "",
    string Email = "",
    string UserName = "",
    string Password = "",
    string ConfirmPassword = "",
    UserRole Role = UserRole.User
);



public enum UserRole { Instructor, Student, User }
