using Microsoft.AspNetCore.Mvc;
using OnlineCourse.Application.Extensions;
using OnlineCourse.Application.Models.Review;
using OnlineCourse.Application.ServiceContracts;

namespace OnlineCourse.Server.Controllers;

[Route("api/v{version:apiVersion}/Courses/course-id/[controller]")]
[ApiController]
[Asp.Versioning.ApiVersion("1.0")]
public class ReviewsController(IReviewService reviewService,
    ICourseService courseService) : ControllerBase
{
    private readonly IReviewService _reviewService = reviewService;
    private readonly ICourseService _courseService = courseService;

    //[HttpGet]
    //public async Task<IActionResult> GetAll(int courseId)
    //{
    //    //var reviews =  _reviewService.GetByCourse(courseId);

    //    return Ok("soon...");
    //}


    [HttpPost]
    public async Task<IActionResult> Create(CreateReviewModel model)
    {
        await _reviewService.CreateAsync(model);

        if (_reviewService.IsValid)
        {
            return Ok("Done");
        }

        _reviewService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }


    [HttpPut("review-id")]
    public async Task<IActionResult> Update(int reviewId, UpdateReviewModel model)
    {
        await _reviewService.UpdateAsync(reviewId, model);


        if (_reviewService.IsValid)
        {
            return Ok("Done");
        }

        _reviewService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }


    [HttpDelete("review-id")]
    public async Task<IActionResult> Delete(int reviewId)
    {
        await _reviewService.DeleteAsync(reviewId);


        if (_reviewService.IsValid)
        {
            return Ok("Done");
        }

        _reviewService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }


    [HttpGet("/api/v1/Courses/top-rated")]
    public async Task<IActionResult> GetTopRatedCourss(int count)
    {
        var courses = await _courseService.GetCoursesTopRatedAsync(count);


        if (_courseService.IsValid)
        {
            return Ok(courses);
        }

        _reviewService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }


    //[HttpGet("api/v1/Courses/course-id/rating")]
    //public async Task<IActionResult> GetRating()
    //{
    //    return Ok("soon...");
    //}
}
