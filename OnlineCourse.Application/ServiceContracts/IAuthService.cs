using OnlineCourse.Application.Dtos;
using OnlineCourse.Application.Models.User;
using StatusGeneric;

namespace OnlineCourse.Application.ServiceContracts;

public interface IAuthService : IStatusGeneric
{
    Task<string> LoginAsync(LoginModel model);
    Task LogoutAsync(Guid userId);
    Task<TokenDto> RefreshTokenAsync(string refreshToken);
    Task<bool> ValidateTokenAsync(string token);
    Task RevokeRefreshTokenAsync(string refreshToken);
    Task<UserDto> GetCurrentUserAsync(string accessToken);
}
