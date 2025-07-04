using Microsoft.AspNetCore.Mvc;
using OnlineCourse.Application.Extensions;
using OnlineCourse.Application.Models.Auth;
using OnlineCourse.Application.Models.RefreshToken;
using OnlineCourse.Application.Models.User;
using OnlineCourse.Application.ServiceContracts;

namespace OnlineCourse.Server.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[Asp.Versioning.ApiVersion("1.0")]
public class AuthesController(IAuthService authService, IUserService userService) : ControllerBase
{
    private readonly IAuthService _authService = authService;
    private readonly IUserService _userService = userService;


    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        await _userService.RegisterAsync(model);
        if (_userService.IsValid)
            return Ok("Done");

        _userService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginModel model)
    {
        var token = await _authService.LoginAsync(model);

        if (_authService.IsValid)
        {
            return Ok(token);
        }

        _authService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }


    [HttpPost("logout")]
    public async Task<IActionResult> Logout(Guid userId)
    {
        await _authService.LogoutAsync(userId);
        if (_authService.IsValid)
        {
            return Ok("done");
        }
        _authService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(RefreshTokenRequestModel model)
    {
        var token = await _authService.RefreshTokenAsync(model);
        if (_authService.IsValid)
        {
            return Ok(token);
        }
        _authService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(string email)
    {
        var response = await _authService.ForgotPassword(email);
        if (_authService.IsValid)
        {
            return Ok(response);
        }

        _authService.CopyToModelState(ModelState);
        return BadRequest(ModelState);

    }



    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
    {
        await _authService.ResetPassword(model);
        if (_authService.IsValid)
        {
            return Ok("Done");
        }

        _authService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }
}
