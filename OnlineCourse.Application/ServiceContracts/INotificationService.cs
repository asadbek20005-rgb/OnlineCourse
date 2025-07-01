using OnlineCourse.Application.Dtos;
using OnlineCourse.Application.Models.Notification;
using StatusGeneric;

namespace OnlineCourse.Application.ServiceContracts;

public interface INotificationService : IStatusGeneric
{
    Task<IEnumerable<NotificationDto>> GetForUserAsync(Guid userId);
    Task MarkAsReadAsync(int notificationId);
    Task SendAsync(CreateNotificationModel model);
}
