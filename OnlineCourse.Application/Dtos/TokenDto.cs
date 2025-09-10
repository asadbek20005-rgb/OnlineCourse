namespace OnlineCourse.Application.Dtos;

public record TokenDto(
    string RefreshToken,
    string AccessToken
);
