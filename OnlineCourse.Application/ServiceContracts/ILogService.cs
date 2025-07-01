using OnlineCourse.Application.Dtos;
using OnlineCourse.Application.Models.Log;
using StatusGeneric;

namespace OnlineCourse.Application.ServiceContracts;

public interface ILogService : IStatusGeneric
{
    Task<IEnumerable<LogDto>> GetAllAsync();
    Task<IEnumerable<LogDto>> GetByUserAsync(Guid userId);
    Task<IEnumerable<LogDto>> SearchAsync(string keyword);
    Task LogAsync(LogRequestModel model);
}
