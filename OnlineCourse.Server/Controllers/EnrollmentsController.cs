using Microsoft.AspNetCore.Mvc;
using OnlineCourse.Application.Extensions;
using OnlineCourse.Application.Models.Student;
using OnlineCourse.Application.ServiceContracts;

namespace OnlineCourse.Server.Controllers;

[Route("api/v{version:apiVersion}/Students/student-id/[controller]")]
[ApiController]
[Asp.Versioning.ApiVersion("1.0")]
public class EnrollmentsController(IStudentService studentService) : ControllerBase
{
    private readonly IStudentService _studentService = studentService;

    [HttpGet]
    public async Task<IActionResult> GetAllEnrollments()
    {
        return Ok("soon...");
    }

    [HttpGet("Courses")]
    public async Task<IActionResult> GetAllCourses()
    {
        //var courses = await _studentService.GetEnrolledCourses();

        return Ok("soon...");

    }

    [HttpDelete("unenroll")]
    public async Task<IActionResult> DeleteStudent(UnEnrollModel model)
    {
        await _studentService.Unenroll(model);

        if (_studentService.IsValid)
            return Ok("Done");

        _studentService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }

    [HttpPost]
    public async Task<IActionResult> Enroll(EnrollRequestModel model)
    {
        await _studentService.EnrollAsync(model);

        if (_studentService.IsValid)
            return Ok("Done");

        _studentService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }
}
