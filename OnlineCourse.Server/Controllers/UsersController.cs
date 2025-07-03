using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using OnlineCourse.Application.Extensions;
using OnlineCourse.Application.Models.User;
using OnlineCourse.Application.ServiceContracts;
using OnlineCourse.Domain.Entities;

namespace OnlineCourse.Server.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[Asp.Versioning.ApiVersion("1.0")]
public class UsersController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _userService.GetAllUserAsync();

        if (_userService.IsValid)
        {
            return Ok(users);
        }

        _userService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }


    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetUserById(Guid userId)
    {
        var user = await _userService.GetUserByIdAsync(userId);
        if (_userService.IsValid)
        {
            return Ok(user);
        }

        _userService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }

    [HttpPut("{userId:guid}")]
    public async Task<IActionResult> UpdateUser(Guid userId, UpdateUserModel model)
    {
        await _userService.UpdateProfileAsync(userId, model);
        if (_userService.IsValid)
        {
            return Ok("Done");
        }

        _userService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }

    [HttpDelete("{userId:guid}")]
    public async Task<IActionResult> DeleteUser(Guid userId)
    {
        await _userService.DeleteAsync(userId);
        if (_userService.IsValid)
        {
            return Ok("Done");
        }

        _userService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }


    [HttpPut("{userId:guid}/role")]
    public async Task<IActionResult> ChangeRole(Guid userId, UserRole newRole)
    {
        await _userService.ChangeRoleAsync(userId, newRole);
        if (_userService.IsValid)
        {
            return Ok("Done");
        }

        _userService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }





    [HttpPut("{userId:guid}/status")]
    public async Task<IActionResult> ChangeStatus(Guid userId, UserStatus newStatus)
    {
        await _userService.ChangeStatusAsync(userId, newStatus);
        if (_userService.IsValid)
        {
            return Ok("Done");
        }

        _userService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }


    [HttpPost("{userId:guid}/avatar")]
    public async Task<IActionResult> UploadImg(Guid userId, IFormFile file)
    {
        return Ok("soon...");
    }

    [HttpPut("{userId:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid userId)
    {
        return Ok("soon...");
    }


    [HttpGet("me")]
    public async Task<IActionResult> GetMyProfile()
    {
        return Ok("soon...");

    }


    [HttpPut("me")]
    public async Task<IActionResult> UpdateMyProfile()
    {
        return Ok("soon...");

    }


    [HttpPut("me/change-password")]
    public async Task<IActionResult> ChangePassword()
    {
        return Ok("soon...");

    }
}
