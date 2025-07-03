using Microsoft.AspNetCore.Mvc;
using OnlineCourse.Application.Extensions;
using OnlineCourse.Application.Models.Lesson;
using OnlineCourse.Application.ServiceContracts;

namespace OnlineCourse.Server.Controllers;

[Route("api/v{version:apiVersion}/Instructors/instructor-id/Courses/course-id/[controller]")]
[ApiController]
[Asp.Versioning.ApiVersion("1.0")]
public class LessonsController(ILessonService lessonService) : ControllerBase
{
    private readonly ILessonService _lessonService = lessonService;

    [HttpPost]
    public async Task<IActionResult> Create(CreateLessonModel model)
    {
        await _lessonService.CreateAsync(model);

        if (_lessonService.IsValid)
        {
            return Ok("Done");
        }

        _lessonService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(int couresId)
    {
        //var lessons = await _lessonService.GetByCourseAsync(couresId);

        return Ok("soon...");
    }

    [HttpPost("/api/v1/Lessons/lesson-id/video")]
    public async Task<IActionResult> UploadVideo(int lessonId, IFormFile file)
    {
        //await _lessonService.UploadVideoAsync(lessonId, file);

        //if (_lessonService.IsValid)
        //{
        //    return Ok("Done");
        //}

        //_lessonService.CopyToModelState(ModelState);
        //return BadRequest(ModelState);


        return Ok("soon...");
    }



}
