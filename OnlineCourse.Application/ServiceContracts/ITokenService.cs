using OnlineCourse.Domain.Entities;
using System.Security.Claims;

namespace OnlineCourse.Application.ServiceContracts;

public interface ITokenService
{
    string GenerateToken(User user);
    string GenerateRefreshToken();
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}
