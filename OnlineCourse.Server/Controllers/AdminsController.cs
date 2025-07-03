using Microsoft.AspNetCore.Mvc;
using OnlineCourse.Application.Extensions;
using OnlineCourse.Application.ServiceContracts;

namespace OnlineCourse.Server.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Asp.Versioning.ApiVersion("1.0")]
public class AdminsController(
    IInstructorService instructorService,
    IUserService userService,
    IPaymentService paymentService) : ControllerBase
{
    private readonly IInstructorService _instructorService = instructorService;
    private readonly IUserService _userService = userService;
    private readonly IPaymentService _paymentService = paymentService;

    [HttpPost("Instructors/instructor-id/approve")]
    public async Task<IActionResult> Approve(int id)
    {
        await _instructorService.ApproveAsync(id);

        if (_instructorService.IsValid)
        {
            return Ok("Done");
        }

        _instructorService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }


    [HttpGet("Users")]
    public async Task<IActionResult> GetUser()
    {
        var users = await _userService.GetAllUserAsync();

        if (_userService.IsValid)
        {
            return Ok("Done");
        }

        _userService.CopyToModelState(ModelState);
        return BadRequest(ModelState);

    }



    [HttpGet("Payments")]
    public async Task<IActionResult> GetPayments()
    {
        return Ok("soon...");
    }


    [HttpPut("Users/{userId:guid}/ban")]
    public async Task<IActionResult> BanUser(Guid userId)
    {
        await _userService.BlockAsync(userId);

        if (_userService.IsValid)
        {
            return Ok("Done");
        }

        _userService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }


    [HttpPut("Users/{userId:guid}/unban")]
    public async Task<IActionResult> unBanUser(Guid userId)
    {
        //await _userService.Un(userId);

        //if (_userService.IsValid)
        //{
        //    return Ok("Done");
        //}

        //_userService.CopyToModelState(ModelState);
        //return BadRequest(ModelState);

        return Ok("soon...");
    }



}
