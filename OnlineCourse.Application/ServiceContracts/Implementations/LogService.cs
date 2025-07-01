using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineCourse.Application.Dtos;
using OnlineCourse.Application.Models.Log;
using OnlineCourse.Application.RepositoryContracts;
using OnlineCourse.Domain.Entities;
using StatusGeneric;

namespace OnlineCourse.Application.ServiceContracts.Implementations;

public class LogService(
    IBaseRepositroy<Log> logRepository,
    IBaseRepositroy<User> userRepository,
    IMapper mapper) : StatusGenericHandler, ILogService
{
    private readonly IBaseRepositroy<Log> _logRepositroy = logRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IBaseRepositroy<User> _userRepository = userRepository;
    public async Task<IEnumerable<LogDto>> GetAllAsync()
    {
        var logs = await _logRepositroy.GetAll().ToListAsync();

        return _mapper.Map<List<LogDto>>(logs);
    }

    public async Task<IEnumerable<LogDto>> GetByUserAsync(Guid userId)
    {
        User? user = await _userRepository.GetByIdAsync(userId);
        if (user is null)
        {
            AddError($"User with id: {userId} is not found");
            return Enumerable.Empty<LogDto>();
        }

        var userLogs = await _logRepositroy.GetAll()
            .Where(x => x.UserID == userId).ToListAsync();


        return _mapper.Map<List<LogDto>>(userLogs);
    }

    public async Task LogAsync(LogRequestModel model)
    {
        User? user = await _userRepository.GetByIdAsync(model.UserId);
        if (user is null)
        {
            AddError($"User with id: {model.UserId} is not found");
            return;
        }

        Log newLog = _mapper.Map<Log>(model);

        await _logRepositroy.AddAsync(newLog);
        await _logRepositroy.SaveChangesAsync();

    }

    public Task<IEnumerable<LogDto>> SearchAsync(string keyword)
    {
        throw new NotImplementedException();
    }
}