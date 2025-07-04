using OnlineCourse.Application.Dtos;
using OnlineCourse.Application.Models.Auth;
using OnlineCourse.Application.Models.RefreshToken;
using OnlineCourse.Application.Models.User;
using StatusGeneric;

namespace OnlineCourse.Application.ServiceContracts;

public interface IAuthService : IStatusGeneric
{
    Task<TokenDto?> LoginAsync(LoginModel model);
    Task LogoutAsync(Guid userId);
    Task<TokenDto?> RefreshTokenAsync(RefreshTokenRequestModel model);
    string ValidateTokenAsync(string token);
    Task RevokeRefreshTokenAsync(string refreshToken);
    Task<UserDto> GetCurrentUserAsync(string accessToken);
    Task ResetPassword(ResetPasswordModel model);
    Task<ForgotPasswordResponse?> ForgotPassword(string myEmail);
}