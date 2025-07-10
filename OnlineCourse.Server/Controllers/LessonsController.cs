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
        var lessons = await _lessonService.GetByCourseAsync(couresId);
        if (_lessonService.IsValid)
        {
            return Ok(lessons);
        }

        _lessonService.CopyToModelState(ModelState);
        return BadRequest(ModelState);

    }

    [HttpPost("/api/v1/Lessons/lesson-id/upload-video")]
    public async Task<IActionResult> UploadVideo(int lessonId, IFormFile file)
    {
        var fileName = await _lessonService.UploadVideoAsync(lessonId, file);

        if (_lessonService.IsValid)
        {
            return Ok(fileName);
        }

        _lessonService.CopyToModelState(ModelState);
        return BadRequest(ModelState);


    }


    [HttpGet("lesson-id")]

    public async Task<IActionResult> GetLessonById([FromQuery] GetLessonByIdModel model)
    {
        var lesson = await _lessonService.GetByIdAsync(model);

        if (_lessonService.IsValid)
        {
            return Ok(lesson);
        }

        _lessonService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }


    [HttpPut("lesson-id")]
    public async Task<IActionResult> Update([FromBody] UpdateWrapperModel model)
    {
        await _lessonService.UpdateAsync(model.model1, model.model2);

        if (_lessonService.IsValid)
        {
            return Ok("Done");
        }

        _lessonService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }


    [HttpDelete("lesson-id")]

    public async Task<IActionResult> Delete(DeleteLessonModel model)
    {
        await _lessonService.DeleteAsync(model);

        if (_lessonService.IsValid)
        {
            return Ok("Done");
        }

        _lessonService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }


    [HttpPost("/api/v1/Lessons/lesson-id/download-video")]
    public async Task<IActionResult> DownloadVideo(DownloadVideoModel model)
    {
        var stream = await _lessonService.DownloadVideoAsync(model);

        if (_lessonService.IsValid)
        {
            return Ok(stream);
        }

        _lessonService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }

    [HttpPost("/api/Instructors/instructor-id/lessons-count")]
    public async Task<IActionResult> GetLessonsCountByInstructorId(GetInstructorLessonsCount model)
    {
        int count = await _lessonService.GetLessonsCountByInstructorIdAsync(model);
        if (_lessonService.IsValid)
        {
            return Ok(count);
        }

        _lessonService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }

}
