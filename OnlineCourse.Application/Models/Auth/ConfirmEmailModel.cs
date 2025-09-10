namespace OnlineCourse.Application.Models.Auth;

public record ConfirmEmailModel(
   string Email,
   int Code
);
