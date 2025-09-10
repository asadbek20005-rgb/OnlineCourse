using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OnlineCourse.Application.Dtos;
using OnlineCourse.Application.Models.Auth;
using OnlineCourse.Application.Models.RefreshToken;
using OnlineCourse.Application.Models.User;
using OnlineCourse.Application.RepositoryContracts;
using OnlineCourse.Domain.Entities;
using StatusGeneric;
using System.Security.Claims;

namespace OnlineCourse.Application.ServiceContracts.Implementations;

public class AuthService(
    IBaseRepositroy<User> _userRepository,
    ITokenService _tokenService,
    ISecurityService _securityService,
    IBaseRepositroy<RefreshToken> _refreshTokenRepository,
    IValidator<LoginModel> _loginValidator,
    IValidator<RefreshTokenRequestModel> _refreshValidator,
    IEmailSenderService _emailSenderService) : StatusGenericHandler, IAuthService
{
    public async Task<ForgotPasswordResponse?> ForgotPassword(string myEmail)
    {
        User? user = await _userRepository.GetAll()
           .Where(x => x.Email == myEmail)
           .FirstOrDefaultAsync();

        if (user is null)
        {
            AddError($"User with email: {myEmail} is not found");
            return null;
        }

        string token = _tokenService.GenerateToken(user);

        var link = $"https://localhost:7163/api/v1/Authes/reset-password?code={token}&email={user.Email}";

        return new ForgotPasswordResponse
        {
            Email = user.Email,
            Token = token,
        };
    }

    public Task<UserDto> GetCurrentUserAsync(string accessToken)
    {
        throw new NotImplementedException();
    }

    public async Task<TokenDto?> LoginAsync(LoginModel model)
    {
        var validatorResult = await _loginValidator.ValidateAsync(model);
        if (!validatorResult.IsValid)
        {
            foreach (var error in validatorResult.Errors)
            {
                AddError($"Validation error: {error.ErrorMessage}");
            }
        }

        User? user = await _userRepository.GetAll()
            .Where(x => x.Email == model.Email || x.UserName == model.UserName)
            .FirstOrDefaultAsync();

        if (user is null)
        {
            AddError($"User with email: {model.Email} or username: {model.UserName} is not found");
            return null;
        }

        bool passwordVerificationResult = _securityService.VerifyPassword(user, user.PasswordHash, model.Password);

        if (passwordVerificationResult is false)
        {
            AddError($"Password verification is failed");
            return null;
        }

        string token = _tokenService.GenerateToken(user);
        string refreshToken = _tokenService.GenerateRefreshToken();

        var newRefreshToken = new RefreshToken
        {
            Token = refreshToken,
            Expires = DateTime.UtcNow.AddMonths(1),
            IsRevoked = false,
            UserId = user.Id
        };

        await _refreshTokenRepository.AddAsync(newRefreshToken);
        await _refreshTokenRepository.SaveChangesAsync();

        return new TokenDto(token, refreshToken);



    }

    public async Task LogoutAsync(Guid userId)
    {
        User? user = await _userRepository.GetByIdAsync(userId);
        if (user is null)
        {
            AddError($"User with id: {userId} is not found");
            return;
        }

        RefreshToken? refreshToken = await _refreshTokenRepository.GetAll()
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync();

        if (refreshToken is null)
        {
            AddError($"Refresh token with user id: {userId} is not found");
            return;
        }


        await _refreshTokenRepository.DeleteAsync(refreshToken);
        await _refreshTokenRepository.SaveChangesAsync();
    }

    public async Task<TokenDto?> RefreshTokenAsync(RefreshTokenRequestModel model)
    {
        var validatorResult = await _refreshValidator.ValidateAsync(model);
        if (!validatorResult.IsValid)
        {
            foreach (var error in validatorResult.Errors)
            {
                AddError($"Validation error: {error.ErrorMessage}");
            }
        }
        ClaimsPrincipal? claimsPrincipal = _tokenService.GetPrincipalFromExpiredToken(model.AccessToken);
        if (claimsPrincipal is null)
        {
            AddError($"Invalid Access Token");
            return null;
        }

        Claim? claim = claimsPrincipal.FindFirst(x => ClaimTypes.Name == x.Type);

        if (claim is null)
        {
            AddError($"Claim is not found");
            return null;
        }

        Guid userId = Guid.Parse(claim.Value);

        RefreshToken? refreshToken = await _refreshTokenRepository.GetAll()
            .Where(x => x.Token == model.RefreshToken && x.UserId == userId)
            .FirstOrDefaultAsync();

        if (refreshToken is null || refreshToken.Expires < DateTime.UtcNow || refreshToken.IsRevoked)
        {
            AddError("Invalid");
            return null;
        }
        refreshToken.IsRevoked = true;

        User? user = await _userRepository.GetByIdAsync(userId);
        if (user is null)
        {
            AddError($"User with id: {userId} is not found");
            return null;
        }

        string accessToken = _tokenService.GenerateToken(user);
        string token = _tokenService.GenerateRefreshToken();

        var newRefreshToken = new RefreshToken
        {
            Token = token,
            Expires = DateTime.UtcNow.AddMonths(1),
            IsRevoked = false,
            UserId = user.Id
        };


        await _refreshTokenRepository.AddAsync(newRefreshToken);
        await _refreshTokenRepository.SaveChangesAsync();


        return new TokenDto(refreshToken.Token, token);

    }

    public async Task ResetPassword(ResetPasswordModel model)
    {

        string emailFromToken = ValidateTokenAsync(model.Token);
        if (emailFromToken == string.Empty)
        {
            AddError("Email is not found from token");
            return;
        }

        User? user = await _userRepository.GetAll()
            .Where(x => x.Email == emailFromToken)
            .FirstOrDefaultAsync();

        if (user is null)
        {
            AddError($"User with email: {emailFromToken} is not found");
            return;
        }


        user.PasswordHash = _securityService.HashPassword(user, model.NewPassword);

        await _userRepository.UpdateAsync(user);
        await _userRepository.SaveChangesAsync();
    }

    public Task RevokeRefreshTokenAsync(string refreshToken)
    {
        throw new NotImplementedException();
    }

    public string ValidateTokenAsync(string token)
    {
        var principals = _tokenService.GetPrincipalFromExpiredToken(token);

        if (principals is null)
        {
            AddError($"Claim principals is not found");
            return string.Empty;
        }
        var email = principals.FindFirst(ClaimTypes.Email)!.Value;
        var resetPassword = principals.FindFirst("ResetPassword")!.Value;

        if (resetPassword != "reset-password")
        {
            return string.Empty;
        }

        return email;
    }
}
