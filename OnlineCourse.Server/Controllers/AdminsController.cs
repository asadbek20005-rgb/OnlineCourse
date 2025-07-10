using Microsoft.AspNetCore.Mvc;
using OnlineCourse.Application.Extensions;
using OnlineCourse.Application.Models.Course;
using OnlineCourse.Application.ServiceContracts;

namespace OnlineCourse.Server.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Asp.Versioning.ApiVersion("1.0")]
public class AdminsController(
    IInstructorService instructorService,
    IUserService userService,
    IPaymentService paymentService,
    ICourseService _courseService) : ControllerBase
{
    private readonly IInstructorService _instructorService = instructorService;
    private readonly IUserService _userService = userService;
    private readonly IPaymentService _paymentService = paymentService;

    [HttpPut("Instructors/instructor-id/approve")]
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
            return Ok(users);
        }

        _userService.CopyToModelState(ModelState);
        return BadRequest(ModelState);

    }



    //[HttpGet("Payments")]
    //public async Task<IActionResult> GetPayments()
    //{
    //    var payments = await _paymentService.GetTotalPaidAsync();
    //    return Ok("soon...");
    //}


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
    public async Task<IActionResult> UnBanUser(Guid userId)
    {
        await _userService.UnBlockAsync(userId);

        if (_userService.IsValid)
        {
            return Ok("Done");
        }

        _userService.CopyToModelState(ModelState);
        return BadRequest(ModelState);

    }


    [HttpPut("Courses/course-id/approve")]
    public async Task<IActionResult> ApproveCourse(ApproveCourseModel model)
    {
        await _courseService.ApproveAsync(model);

        if (_courseService.IsValid)
            return Ok("Done");

        _courseService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }


    [HttpPut("Courses/course-id/reject")]
    public async Task<IActionResult> RejectCourse(RejectCourseModel model)
    {
        await _courseService.RejectAsync(model);

        if (_courseService.IsValid)
            return Ok("Done");

        _courseService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }


}
