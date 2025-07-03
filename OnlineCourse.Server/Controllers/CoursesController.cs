using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using OnlineCourse.Application.Extensions;
using OnlineCourse.Application.Models.Course;
using OnlineCourse.Application.Models.Favourite;
using OnlineCourse.Application.Models.Student;
using OnlineCourse.Application.ServiceContracts;

namespace OnlineCourse.Server.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/Instructors/instructor-id/[controller]")]
[ApiVersion("1.0")]
public class CoursesController(
    ICourseService courseService,
    IFavouriteService favouriteService,
    IStudentService studentService) : ControllerBase
{
    private readonly ICourseService _courseService = courseService;
    private readonly IFavouriteService _favouriteService = favouriteService;
    private readonly IStudentService _studentService = studentService;


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCourseModel model)
    {
        await _courseService.CreateAsync(model);
        if (_courseService.IsValid)
            return Ok("Done");

        _courseService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var courses = await _courseService.GetAllCourseAsync();
        if (_courseService.IsValid)
            return Ok(courses);

        _courseService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }

    [HttpGet("course-id")]
    public async Task<IActionResult> GetById(int courseId)
    {
        var course = await _courseService.GetCourseByIdAsync(courseId);

        if (_courseService.IsValid)
            return Ok(course);

        _courseService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }


    [HttpPut("course-id")]
    public async Task<IActionResult> Update(int courseId, [FromBody] UpdateCourseModel model)
    {
        await _courseService.UpdateAsync(courseId, model);

        if (_courseService.IsValid)
            return Ok("Done");

        _courseService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }


    [HttpDelete("course-id")]
    public async Task<IActionResult> Delete(int courseId)
    {
        await _courseService.DeleteAsync(courseId);
        if (_courseService.IsValid)
            return Ok("Done");

        _courseService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }

    [HttpGet("category")]
    public async Task<IActionResult> Filter([FromQuery] int categoryId)
    {
        var courses = await _courseService.GetCoursesByCategoryAsync(categoryId);

        if (_courseService.IsValid)
            return Ok(courses);

        _courseService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }

    [HttpGet("rating")]
    public async Task<IActionResult> GetTopRated([FromQuery] int count)
    {
        var courses = await _courseService.GetCoursesTopRatedAsync(count);
        if (_courseService.IsValid)
            return Ok(courses);

        _courseService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }

    [HttpPut("api/v{version:apiVersion}/Courses/publish")]
    public async Task<IActionResult> Publish(int courseId)
    {
        await _courseService.PublishAsync(courseId);

        if (_courseService.IsValid)
            return Ok("Done");

        _courseService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }

    [HttpPut("api/v{version:apiVersion}/Courses/unpublish")]
    public async Task<IActionResult> UnPublish(int courseId)
    {
        return Ok("soon...");
    }


    [HttpPost("api/v{version:apiVersion}/Courses/favorite")]
    public async Task<IActionResult> AddFavourite(AddToFavouriteRequestModel model)
    {
        await _favouriteService.AddToFavouriteAsync(model);
        if (_courseService.IsValid)
            return Ok("Done");

        _courseService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }

    [HttpDelete("api/v{version:apiVersion}/Courses/favorite")]
    public async Task<IActionResult> DeleteFavorite(int id)
    {
        await _favouriteService.RemoveFromFavouritesAsync(id);

        if (_courseService.IsValid)
            return Ok("Done");

        _courseService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }



    [HttpGet("api/v{version:apiVersion}/Users/me/favorites")]
    public async Task<IActionResult> GetMyFavorites(Guid userId)
    {
        var courses = await _favouriteService.GetByUserAsync(userId);

        if (_courseService.IsValid)
            return Ok(courses);

        _courseService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }



    [HttpGet("api/v{version:apiVersion}/Students/student-id/progress")]
    public async Task<IActionResult> GetStudentProgresses(GetProgressRequestModel model)
    {
        var progresses = await _studentService.GetProgressAsync(model);

        if (_courseService.IsValid)
            return Ok(progresses);

        _courseService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }



}
