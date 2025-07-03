using Microsoft.AspNetCore.Mvc;
using OnlineCourse.Application.Extensions;
using OnlineCourse.Application.Models.Notification;
using OnlineCourse.Application.ServiceContracts;

namespace OnlineCourse.Server.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Asp.Versioning.ApiVersion("1.0")]
public class NotificationsController(INotificationService notificationService) : ControllerBase
{
    private readonly INotificationService _notificationService = notificationService;

    [HttpGet]
    public async Task<IActionResult> GetALl(Guid userId)
    {
        var notifications = await _notificationService.GetForUserAsync(userId);


        if (_notificationService.IsValid)
        {
            return Ok(notifications);
        }

        _notificationService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }


    [HttpPut("notification-id")]
    public async Task<IActionResult> Read(int notificationId)
    {
        await _notificationService.MarkAsReadAsync(notificationId);


        if (_notificationService.IsValid)
        {
            return Ok("Done");
        }

        _notificationService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }


    [HttpPost]
    public async Task<IActionResult> Send(CreateNotificationModel model)
    {
        await _notificationService.SendAsync(model);


        if (_notificationService.IsValid)
        {
            return Ok("Done");
        }

        _notificationService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }


}
