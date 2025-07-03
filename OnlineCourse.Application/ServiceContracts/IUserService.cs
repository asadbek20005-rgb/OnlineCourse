using OnlineCourse.Application.Dtos;
using OnlineCourse.Application.Models.User;
using OnlineCourse.Domain.Entities;
using StatusGeneric;

namespace OnlineCourse.Application.ServiceContracts;

public interface IUserService : IStatusGeneric
{
    Task<UserDto?> GetUserByIdAsync(Guid userId);
    Task<UserDto?> GetUserByEmailAsync(string email);
    Task<IEnumerable<UserDto>> GetAllUserAsync();
    Task RegisterAsync(RegisterModel model);
    Task<bool> ConfirmEmailAsync(Guid userId, string token);
    Task ChangePasswordAsync(ChangePasswordModel model);
    Task UpdateProfileAsync(Guid userId, UpdateUserModel model);
    Task ChangeRoleAsync(Guid userId, UserRole newRole);
    Task ChangeStatusAsync(Guid userId, UserStatus newStatus);
    Task BlockAsync(Guid userId);
    Task DeleteAsync(Guid userId);
    Task<bool> EmailExistAsync(string email);

}

//Task<ResultDto<PaginatedList<UserDto>>> GetPagedAsync(UserFilterDto filter);