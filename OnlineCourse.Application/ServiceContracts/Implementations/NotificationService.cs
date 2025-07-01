using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineCourse.Application.Dtos;
using OnlineCourse.Application.Models.Notification;
using OnlineCourse.Application.RepositoryContracts;
using OnlineCourse.Domain.Entities;
using StatusGeneric;

namespace OnlineCourse.Application.ServiceContracts.Implementations;

public class NotificationService(
    IBaseRepositroy<Notification> notificationRepository,
    IBaseRepositroy<User> userRepositroy,
    IMapper mapper) : StatusGenericHandler, INotificationService
{
    private readonly IBaseRepositroy<Notification> _notificationRepository = notificationRepository;
    private readonly IBaseRepositroy<User> _userRepository = userRepositroy;
    private readonly IMapper _mapper = mapper;


    public async Task<IEnumerable<NotificationDto>> GetForUserAsync(Guid userId)
    {
        User? user = await _userRepository.GetByIdAsync(userId);
        if (user is null)
        {
            AddError($"User with id: {userId} is not found");
            return Enumerable.Empty<NotificationDto>();
        }

        var userNotifications = await _notificationRepository.GetAll()
            .Where(x => x.UserID == user.Id)
            .ToListAsync();

        if (userNotifications is null)
        {
            AddError($"User's with id: {userId} notifications is not found");
            return Enumerable.Empty<NotificationDto>();
        }

        return _mapper.Map<List<NotificationDto>>(userNotifications);
    }

    public async Task MarkAsReadAsync(int notificationId)
    {
        Notification? notification = await _notificationRepository.GetByIdAsync(notificationId);

        if (notification is null)
        {
            AddError($"Notification with id: {notificationId} is not found");
            return;
        }

        notification.IsRead = true;
        await _notificationRepository.UpdateAsync(notification);
        await _notificationRepository.SaveChangesAsync();
    }

    public async Task SendAsync(CreateNotificationModel model)
    {
        User? user = await _userRepository.GetByIdAsync(model.UserID);
        if (user is null)
        {
            AddError($"User with id: {model.UserID} is not found");
            return;
        }


        Notification newNotification = _mapper.Map<Notification>(model);

        await _notificationRepository.AddAsync(newNotification);
        await _notificationRepository.SaveChangesAsync();
    }
}
