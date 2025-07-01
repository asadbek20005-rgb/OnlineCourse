using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OnlineCourse.Application.Models.Jwt;
using OnlineCourse.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace OnlineCourse.Application.ServiceContracts.Implementations;

public class TokenService(IConfiguration configuration) : ITokenService
{
    private readonly JwtModel _model = configuration.GetSection("Jwt").Get<JwtModel>()!;
    public string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    public string GenerateToken(User user)
    {
        var signingKey = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_model.Key)), SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
        };

        var token = new JwtSecurityTokenHandler()
            .WriteToken(new JwtSecurityToken
            (
                issuer: _model.Issue,
                audience: _model.Audiance,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: signingKey
            ));


        return token;
    }

    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = _model.Issue,
            ValidateAudience = true,
            ValidAudience = _model.Audiance,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_model.Key!))
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
    }
}
