using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCourse.Application.Extensions;
using OnlineCourse.Application.Models.User;
using OnlineCourse.Application.ServiceContracts;
using OnlineCourse.Domain.Entities;
namespace OnlineCourse.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserService userService, IUserHelperService _userHelperService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [Authorize(Roles = "Admin")]
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

    [Authorize(Roles = "Admin")]
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
    [Authorize(Roles = "Admin")]
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
    [Authorize(Roles = "Admin")]
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

    [Authorize(Roles = "Admin")]
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




    [Authorize(Roles = "Admin")]
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


    [HttpPost("{userId:guid}/img")]
    public async Task<IActionResult> UploadImg(Guid userId, IFormFile file)
    {
        await _userService.UploadImgAsync(userId, file);

        if (_userService.IsValid)
        {
            return Ok("Done");
        }

        _userService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }

    [Authorize(Roles = "Instructor, Student, User")]
    [HttpGet("me")]
    public async Task<IActionResult> GetMyProfile()
    {
        var userId = _userHelperService.GetUserId();
        var user = await _userService.GetUserProfileAsync(userId);

        if (_userService.IsValid)
        {
            return Ok(user);
        }

        _userService.CopyToModelState(ModelState);
        return BadRequest(ModelState);

    }

    [Authorize(Roles = "Instructor, Student")]
    [HttpPut("me")]
    public async Task<IActionResult> UpdateMyProfile(Guid userId, UpdateUserModel model)
    {
        await _userService.UpdateProfileAsync(userId, model);

        if (_userService.IsValid)
        {
            return Ok("Done");
        }

        _userService.CopyToModelState(ModelState);
        return BadRequest(ModelState);

    }

}
