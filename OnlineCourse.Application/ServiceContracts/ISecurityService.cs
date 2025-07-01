using OnlineCourse.Domain.Entities;

namespace OnlineCourse.Application.ServiceContracts;

public interface ISecurityService
{
    string HashPassword(User user,string password);
    bool VerifyPassword(User user, string hash, string provided);

}
