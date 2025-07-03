using Microsoft.AspNetCore.Mvc;
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
}
