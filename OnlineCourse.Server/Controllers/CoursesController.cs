using Microsoft.AspNetCore.Mvc;
using OnlineCourse.Application.Extensions;
using OnlineCourse.Application.Models.Course;
using OnlineCourse.Application.Models.Favourite;
using OnlineCourse.Application.Models.Pagination;
using OnlineCourse.Application.Models.Student;
using OnlineCourse.Application.ServiceContracts;

namespace OnlineCourse.Server.Controllers;

[Route("api/v{version:apiVersion}/Instructors/instructor-id/[controller]")]
[ApiController]
[Asp.Versioning.ApiVersion("1.0")]
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

    [HttpGet("/api/v1/Courses/category")]
    public async Task<IActionResult> Filter([FromQuery] int categoryId)
    {
        var courses = await _courseService.GetCoursesByCategoryAsync(categoryId);

        if (_courseService.IsValid)
            return Ok(courses);

        _courseService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }

    [HttpGet("/api/v1/Courses/rating")]
    public async Task<IActionResult> GetTopRated([FromQuery] int count)
    {
        var courses = await _courseService.GetCoursesTopRatedAsync(count);
        if (_courseService.IsValid)
            return Ok(courses);

        _courseService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }

    [HttpPut("publish")]
    public async Task<IActionResult> Publish(PublishModel model)
    {
        await _courseService.PublishAsync(model);

        if (_courseService.IsValid)
            return Ok("Done");

        _courseService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }

    [HttpPut("unpublish")]
    public async Task<IActionResult> UnPublish(UnPublishModel model)
    {
        await _courseService.UnPublishAsync(model);

        if (_courseService.IsValid)
            return Ok("Done");

        _courseService.CopyToModelState(ModelState);
        return BadRequest(ModelState);

    }


    [HttpPost("/api/v1/Users/user-id/Courses/course-id/favorite")]
    public async Task<IActionResult> AddFavourite(AddToFavouriteRequestModel model)
    {
        await _favouriteService.AddToFavouriteAsync(model);
        if (_courseService.IsValid)
            return Ok("Done");

        _courseService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }

    [HttpDelete("/api/v1/Users/user-id/Courses/course-id/favorite")]
    public async Task<IActionResult> DeleteFavorite(int favoriteId)
    {
        await _favouriteService.RemoveFromFavouritesAsync(favoriteId);

        if (_favouriteService.IsValid)
            return Ok("Done");

        _courseService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }



    [HttpGet("/api/v1/Users/user-id/Courses/favorites")]
    public async Task<IActionResult> GetMyFavorites(Guid userId)
    {
        var courses = await _favouriteService.GetByUserAsync(userId);

        if (_courseService.IsValid)
            return Ok(courses);

        _courseService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }


    [HttpGet("/api/v1/Courses/pagination")]
    public async Task<IActionResult> GetCoursesPagination([FromQuery] PaginationModel model)
    {
        var courses = await _courseService.GetCoursesByPagination(model);

        if (_courseService.IsValid)
            return Ok(courses);

        _courseService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }

    [HttpGet("/api/v1/Courses/price")]
    public async Task<IActionResult> GetByPrice([FromQuery] GetCoursesByPriceModel model)
    {
        var courses = await _courseService.GetCoursesByPriceAsync(model);


        if (_courseService.IsValid)
            return Ok(courses);

        _courseService.CopyToModelState(ModelState);
        return BadRequest(ModelState);

    }


    [HttpGet("/api/v1/Courses/level")]
    public async Task<IActionResult> GetByLevel([FromQuery] GetCoursesByLevelModel model)
    {
        var courses = await _courseService.GetCoursesByLevelAsync(model);


        if (_courseService.IsValid)
            return Ok(courses);

        _courseService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }


    [HttpGet("/api/v1/Categories/category-id/course-count")]
    public async Task<IActionResult> GetCourseCount(GetCourseCountByCategoryIdModel model)
    {
        int count = await _courseService.GetCourseCountByCategoryIdAsync(model);


        if (_courseService.IsValid)
            return Ok(count);

        _courseService.CopyToModelState(ModelState);
        return BadRequest(ModelState);

    }


    [HttpGet("/api/v1/Courses/count")]
    public async Task<IActionResult> GetCoursesCount()
    {
        int count = await _courseService.GetTotalCourseCountAsync();

        if (_courseService.IsValid)
            return Ok(count);

        _courseService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }


    [HttpGet("/api/v1/Courses/published")]
    public async Task<IActionResult> GetPulishedCourses()
    {
        var courses = await _courseService.GetPublishedCoursesAsync();

        if (_courseService.IsValid)
            return Ok(courses);

        _courseService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }

}