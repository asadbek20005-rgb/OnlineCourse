using Microsoft.AspNetCore.Http;
using OnlineCourse.Application.Dtos;
using OnlineCourse.Application.Models.Auth;
using OnlineCourse.Application.Models.User;
using OnlineCourse.Domain.Entities;
using StatusGeneric;
using UserRole = OnlineCourse.Domain.Entities.UserRole;

namespace OnlineCourse.Application.ServiceContracts;

public interface IUserService : IStatusGeneric
{
    Task<UserDto?> GetUserByIdAsync(Guid userId);
    Task<UserDto?> GetUserByEmailAsync(string email);
    Task<IEnumerable<UserDto>> GetAllUserAsync();
    Task<string> RegisterAsync(RegisterModel model);
    Task<bool> ConfirmEmailAsync(ConfirmEmailModel model);
    Task ChangePasswordAsync(ChangePasswordModel model);
    Task UpdateProfileAsync(Guid userId, UpdateUserModel model);
    Task ChangeRoleAsync(Guid userId, UserRole newRole);
    Task ChangeStatusAsync(Guid userId, UserStatus newStatus);
    Task BlockAsync(Guid userId);
    Task UnBlockAsync(Guid userId);
    Task DeleteAsync(Guid userId);
    Task<bool> EmailExistAsync(string email);
    Task UploadImgAsync(Guid userId, IFormFile file);
    Task<Stream> DownloadImgAsync(Guid userId, string fileName);
    Task<UserDto?> GetUserProfileAsync(Guid userId);
}