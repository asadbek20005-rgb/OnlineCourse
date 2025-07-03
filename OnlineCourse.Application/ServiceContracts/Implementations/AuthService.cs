using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OnlineCourse.Application.Dtos;
using OnlineCourse.Application.Models.RefreshToken;
using OnlineCourse.Application.Models.User;
using OnlineCourse.Application.RepositoryContracts;
using OnlineCourse.Domain.Entities;
using StatusGeneric;
using System.Security.Claims;

namespace OnlineCourse.Application.ServiceContracts.Implementations;

public class AuthService(
    IBaseRepositroy<User> userRepository,
    ITokenService tokenService,
    ISecurityService securityService,
    IBaseRepositroy<RefreshToken> refreshTokenRepository,
    IValidator<LoginModel> loginValidator,
    IValidator<RefreshTokenRequestModel> refreshValidator) : StatusGenericHandler, IAuthService
{
    private readonly IBaseRepositroy<User> _userRepository = userRepository;
    private readonly ITokenService _tokenService = tokenService;
    private readonly ISecurityService _securityService = securityService;
    private readonly IBaseRepositroy<RefreshToken> _refreshTokenRepository = refreshTokenRepository;
    private readonly IValidator<LoginModel> _loginValidator = loginValidator;
    private readonly IValidator<RefreshTokenRequestModel> _refreshValidator = refreshValidator;
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


        return new TokenDto
        {
            AccessToken = token,
            RefreshToken = refreshToken
        };


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


        return new TokenDto
        {
            AccessToken = accessToken,
            RefreshToken = token,
        };

    }

    public Task RevokeRefreshTokenAsync(string refreshToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ValidateTokenAsync(string token)
    {
        throw new NotImplementedException();
    }
}
