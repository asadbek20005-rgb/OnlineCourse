using OnlineCourse.Application.Dtos;
using OnlineCourse.Application.Models.Logs;
using StatusGeneric;

namespace OnlineCourse.Application.ServiceContracts;

public interface IActivityLogService : IStatusGeneric
{
    Task<IEnumerable<ActivityLogDto>> GetForUserAsync(Guid userId);
    Task<IEnumerable<ActivityLogDto>> GetByTargetAsync(GetByTargetRequestModel model);
    Task LogActivityAsync(LogActivityRequestModel model);
}
