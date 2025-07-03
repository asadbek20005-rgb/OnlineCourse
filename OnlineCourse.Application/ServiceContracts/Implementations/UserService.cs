using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OnlineCourse.Application.Dtos;
using OnlineCourse.Application.Models.User;
using OnlineCourse.Application.RepositoryContracts;
using OnlineCourse.Domain.Entities;
using StatusGeneric;

namespace OnlineCourse.Application.ServiceContracts.Implementations;

public class UserService(
    IBaseRepositroy<User> userRepositroy,
    IMapper mapper,
    ISecurityService securityService,
    IValidator<RegisterModel> registerValidator) : StatusGenericHandler, IUserService
{
    private readonly IBaseRepositroy<User> _userRepository = userRepositroy;
    private readonly IMapper _mapper = mapper;
    private readonly ISecurityService _securityService = securityService;
    private readonly IValidator<RegisterModel> _registerValidator = registerValidator;
    public async Task BlockAsync(Guid userId)
    {
        User? user = await _userRepository.GetByIdAsync(userId);
        if (user is null)
        {
            AddError($"User with Id: {userId} is not found");
            return;
        }
        user.IsBlocked = true;
        await _userRepository.UpdateAsync(user);
        await _userRepository.SaveChangesAsync();
    }

    public Task ChangePasswordAsync(ChangePasswordModel model)
    {
        throw new NotImplementedException();
    }

    public async Task ChangeRoleAsync(Guid userId, UserRole newRole)
    {
        User? user = await _userRepository.GetByIdAsync(userId);
        if (user is null)
        {
            AddError($"User with Id: {userId} is not found");
            return;
        }

        user.Role = newRole;
        await _userRepository.UpdateAsync(user);
        await _userRepository.SaveChangesAsync();
    }

    public async Task ChangeStatusAsync(Guid userId, UserStatus newStatus)
    {
        User? user = await _userRepository.GetByIdAsync(userId);
        if (user is null)
        {
            AddError($"User with Id: {userId} is not found");
            return;
        }

        user.Status = newStatus;
        await _userRepository.UpdateAsync(user);
        await _userRepository.SaveChangesAsync();
    }

    public Task<bool> ConfirmEmailAsync(Guid userId, string token)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(Guid userId)
    {
        User? user = await _userRepository.GetByIdAsync(userId);
        if (user is null)
        {
            AddError($"User with Id: {userId} is not found");
            return;
        }

        await _userRepository.DeleteAsync(user);
        await _userRepository.SaveChangesAsync();
    }

    public async Task<bool> EmailExistAsync(string email)
    {
        return await (_userRepository.GetAll()).AnyAsync(user => user.Email == email);
    }

    public async Task<IEnumerable<UserDto>> GetAllUserAsync()
    {
        var users = await _userRepository.GetAll().ToListAsync();
        return _mapper.Map<List<UserDto>>(users);
    }

    public async Task<UserDto?> GetUserByEmailAsync(string email)
    {
        User? user = await (_userRepository.GetAll())
            .Where(user => user.Email == email)
            .SingleOrDefaultAsync();

        if (user is null)
        {
            AddError($"User with email: {email} is not found");
            return null;
        }

        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto?> GetUserByIdAsync(Guid userId)
    {
        User? user = await _userRepository.GetByIdAsync(userId);
        if (user is null)
        {
            AddError($"User with Id: {userId} is not found");
            return null;
        }
        return _mapper.Map<UserDto>(user);
    }

    public async Task RegisterAsync(RegisterModel model)
    {
        var validationResult = await _registerValidator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                AddError($"Validation error: {error}");
            }
            return;
        }

        var user = await GetUserByEmailAsync(model.Email);
        if (user is not null)
        {
            AddError($"User with email: {user.Email} is already exist");
            return;
        }

        var newUser = _mapper.Map<User>(model);
        newUser.PasswordHash = _securityService.HashPassword(newUser, model.Password);
        await _userRepository.AddAsync(newUser);
        await _userRepository.SaveChangesAsync();
    }

    public async Task UpdateProfileAsync(Guid userId, UpdateUserModel model)
    {
        User? user = await _userRepository.GetByIdAsync(userId);
        if (user is null)
        {
            AddError($"User with Id: {userId} is not found");
            return;
        }

        User updatedUser = _mapper.Map(model, user);
        await _userRepository.UpdateAsync(updatedUser);
        await _userRepository.SaveChangesAsync();
    }



    //private async Task<User> GetUserById(Guid userId)
    //{
    //    User? user = await _userRepository.GetByIdAsync(userId);
    //    if (user is null)
    //    {
    //        AddError($"User with Id: {userId} is not found");
    //        return null;
    //    }
    //    return user;
    //}
}
