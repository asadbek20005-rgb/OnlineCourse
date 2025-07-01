using Microsoft.AspNetCore.Identity;
using OnlineCourse.Domain.Entities;

namespace OnlineCourse.Application.ServiceContracts.Implementations;

public class SecurityService : ISecurityService
{
    public string HashPassword(User user, string password)
    {
        return new PasswordHasher<User>().HashPassword(user, password);

    }

    public bool VerifyPassword(User user, string hash, string provided)
    {
        PasswordVerificationResult
            verificationResult = new PasswordHasher<User>().VerifyHashedPassword(user, hash, provided);

        if (verificationResult is PasswordVerificationResult.Failed)
            return false;

        return true;

    }
}
