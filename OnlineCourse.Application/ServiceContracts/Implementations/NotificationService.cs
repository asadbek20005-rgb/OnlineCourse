using AutoMapper;
using FluentValidation;
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
    IMapper mapper,
    IValidator<CreateNotificationModel> validator) : StatusGenericHandler, INotificationService
{
    private readonly IBaseRepositroy<Notification> _notificationRepository = notificationRepository;
    private readonly IBaseRepositroy<User> _userRepository = userRepositroy;
    private readonly IMapper _mapper = mapper;
    private readonly IValidator<CreateNotificationModel> _validator = validator;

    public async Task DeleteAsync(int id)
    {
        Notification? notification = await _notificationRepository.GetByIdAsync(id);

        if (notification is null)
        {
            AddError($"Notification with id: {id} is not found");
            return;
        }

        await _notificationRepository.DeleteAsync(notification);
        await _notificationRepository.SaveChangesAsync();
    }

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

    public async Task MarkAllReadAsync(Guid userId)
    {
        User? user = await _userRepository.GetByIdAsync(userId);

        if (user is null)
        {
            AddError($"User with id: {userId} is not found");
            return;
        }


        var notifications = await _notificationRepository.GetAll()
            .Where(x => x.UserID == user.Id).ToListAsync();

        foreach (var notification in notifications)
        {
            notification.IsRead = true;
        }
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
        var validatorResult = await _validator.ValidateAsync(model);
        if (!validatorResult.IsValid)
        {
            foreach (var error in validatorResult.Errors)
            {
                AddError($"Validation error: {error.ErrorMessage}");
            }
        }
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
