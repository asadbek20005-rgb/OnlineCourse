using OnlineCourse.Application.Dtos;

namespace OnlineCourse.Application.ServiceContracts;

public interface IActivityLogService
{
    IEnumerable<ActivityLogDto> GetForUser(Guid userId);
    IEnumerable<ActivityLogDto> GetByTarget(int target, int targetId);
    Task LogActivityAsync(Guid userId, string action, int target, int targetId);
}
