using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OnlineCourse.Application.Dtos;
using OnlineCourse.Application.Models.Log;
using OnlineCourse.Application.RepositoryContracts;
using OnlineCourse.Domain.Entities;
using StatusGeneric;

namespace OnlineCourse.Application.ServiceContracts.Implementations;

public class ActivityLogService(IBaseRepositroy<ActivityLog> activityLogRepository,
    IBaseRepositroy<User> userRepository,
    IMapper mapper,
    IValidator<GetByTargetRequestModel> getValidator,
    IValidator<LogActivityRequestModel> logValidator) : StatusGenericHandler, IActivityLogService
{
    private readonly IBaseRepositroy<ActivityLog> _activityLogRepository = activityLogRepository;
    private readonly IBaseRepositroy<User> _userRepository = userRepository;
    private readonly IValidator<GetByTargetRequestModel> _getValidator = getValidator;
    private readonly IValidator<LogActivityRequestModel> _logValidator = logValidator;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<ActivityLogDto>> GetByTargetAsync(GetByTargetRequestModel model)
    {
        var validatorResult = await _getValidator.ValidateAsync(model);
        if (!validatorResult.IsValid)
        {
            foreach (var error in validatorResult.Errors)
            {
                AddError($"Validation error: {error.ErrorMessage}");
            }
        }
        var logs = await _activityLogRepository.GetAll()
            .Where(x => x.TargetTableId == model.TargetId && x.TargetTable == model.Target)
            .ToListAsync();

        return _mapper.Map<List<ActivityLogDto>>(logs);
    }

    public async Task<IEnumerable<ActivityLogDto>> GetForUserAsync(Guid userId)
    {
        User? user = await _userRepository.GetByIdAsync(userId);
        if (user is null)
        {
            AddError($"User with id: {userId} is not found");
            return Enumerable.Empty<ActivityLogDto>();
        }

        var logs = await _activityLogRepository.GetAll()
            .Where(x => x.UserID == user.Id).ToListAsync();

        return _mapper.Map<List<ActivityLogDto>>(logs);


    }

    public async Task LogActivityAsync(LogActivityRequestModel model)
    {
        var validatorResult = await _logValidator.ValidateAsync(model);
        if (!validatorResult.IsValid)
        {
            foreach (var error in validatorResult.Errors)
            {
                AddError($"Validation error: {error.ErrorMessage}");
            }
        }
        User? user = await _userRepository.GetByIdAsync(model.UserId);
        if (user is null)
        {
            AddError($"User with id: {model.UserId} is not found");
            return;
        }


        var newActivityLog = new ActivityLog
        {
            UserID = user.Id,
            TargetTable = model.Target,
            TargetTableId = model.TargetId
        };

        await _activityLogRepository.AddAsync(newActivityLog);
    }
}
