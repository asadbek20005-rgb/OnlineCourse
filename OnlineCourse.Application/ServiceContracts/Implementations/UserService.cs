using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OnlineCourse.Application.Dtos;
using OnlineCourse.Application.Models.Auth;
using OnlineCourse.Application.Models.Email;
using OnlineCourse.Application.Models.Minio;
using OnlineCourse.Application.Models.User;
using OnlineCourse.Application.RepositoryContracts;
using OnlineCourse.Domain.Entities;
using StatusGeneric;

namespace OnlineCourse.Application.ServiceContracts.Implementations;

public class UserService(
    IBaseRepositroy<User> userRepositroy,
    IMapper mapper,
    ISecurityService securityService,
    IValidator<RegisterModel> registerValidator,
    IRedisService _redisService,
    IBaseRepositroy<EmailOtp> _otpRepository,
    IEmailSenderService _emailSenderService,
    IMinioService _minioService) : StatusGenericHandler, IUserService
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

    public async Task<bool> ConfirmEmailAsync(ConfirmEmailModel model)
    {
        int code = await _redisService.GetAsync<int>(model.Email);

        if (code != model.Code)
        {
            AddError($"Code {model.Code} or email {model.Email} is invalid");
            return false;
        }

        bool expiredCode = await _otpRepository.GetAll()
            .AnyAsync(x => x.Code == model.Code && x.IsExpired == true);

        if (expiredCode)
        {
            AddError($"Code {model.Code} is already expired");
            return false;
        }

        var newOtp = new EmailOtp
        {
            Email = model.Email,
            Code = model.Code,
            IsExpired = true
        };

        await _otpRepository.AddAsync(newOtp);
        await _otpRepository.SaveChangesAsync();

        User? user = await _redisService.GetAsync<User>("user");

        if (user is null)
        {
            AddError("User is not found");
            return false;
        }

        user.EmailConfirmed = true;

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();
        return true;

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

    public async Task<Stream> DownloadImgAsync(Guid userId, string fileName)
    {
        User? user = await _userRepository.GetByIdAsync(userId);

        if (user is null)
        {
            AddError($"User with id: {userId} is not found");
            return Stream.Null;
        }

        Stream stream = await _minioService.DownloadFileAsync(fileName);

        return stream;
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

    public async Task<UserDto?> GetUserProfileAsync(Guid userId)
    {
        User? user = await _userRepository.GetByIdAsync(userId);

        if (user is null)
        {
            AddError($"User with id: {userId} is not found");
            return null;
        }

        return _mapper.Map<UserDto>(user);
    }

    public async Task<string> RegisterAsync(RegisterModel model)
    {
        var validationResult = await _registerValidator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                AddError($"Validation error: {error}");
            }
            return string.Empty;
        }

        var query = _userRepository.GetAll();

        bool emailExist = await query.AnyAsync(x => x.Email == model.Email);

        if (emailExist)
        {
            AddError($"User with email: {model.Email} is already exist");
            return string.Empty;
        }

        bool usernameExist = await query.AnyAsync(x => x.UserName == model.UserName);

        if (usernameExist)
        {
            AddError($"User with username: {model.UserName} is already exist");
            return string.Empty;
        }

        var newUser = _mapper.Map<User>(model);
        newUser.PasswordHash = _securityService.HashPassword(newUser, model.Password);

        await _redisService.SetAsync("user", newUser);

        int code = new Random().Next(1111, 9999);
        await _redisService.SetAsync(model.Email, code);
        string message = $"The verification code is {code}. Code is expired in 1 minutes";

        var senderModel = new EmailSenderModel
        {
            To = model.Email,
            Body = $"Tasdiqlash kodi: {code}. Kod muddati 1 daqiqada tugaydi",
            Subject = "Tasdiqlash"

        };
        await _emailSenderService.SendAsync(senderModel);
        return message;
    }

    public async Task UnBlockAsync(Guid userId)
    {
        User? user = await _userRepository.GetByIdAsync(userId);

        if (user is null)
        {
            AddError($"User with id: {userId} is not found");
            return;
        }

        user.IsBlocked = false;
        await _userRepository.UpdateAsync(user);
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

    public async Task UploadImgAsync(Guid userId, IFormFile file)
    {
        User? user = await _userRepository.GetByIdAsync(userId);

        if (user is null)
        {
            AddError($"User with id: {userId} is not found");
            return;
        }

        var (fileName, contentType, size, data) = await SaveFileDetails(file);

        var uploadFileModel = new UploadFileModel
        {
            FileName = fileName,
            ContentType = contentType,
            Size = size,
            Data = data
        };

        await _minioService.UploadFileAsync(uploadFileModel);
    }


    private async Task<(string FileName, string ContentType, long Size, MemoryStream Data)> SaveFileDetails(IFormFile file)
    {
        var fileName = Guid.NewGuid().ToString();
        string contentType = file.ContentType;
        long size = file.Length;

        var data = new MemoryStream();
        await file.CopyToAsync(data);

        return (fileName, contentType, size, data);
    }




}
