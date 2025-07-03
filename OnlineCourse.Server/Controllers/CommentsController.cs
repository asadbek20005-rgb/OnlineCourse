using Microsoft.AspNetCore.Mvc;
using OnlineCourse.Application.Extensions;
using OnlineCourse.Application.Models.Comment;
using OnlineCourse.Application.ServiceContracts;

namespace OnlineCourse.Server.Controllers;

[Route("api/v{version:apiVersion}/Lesson/lesson-id/[controller]")]
[ApiController]
[Asp.Versioning.ApiVersion("1.0")]
public class CommentsController(ICommentService commentService) : ControllerBase
{
    private readonly ICommentService _commentService = commentService;


    [HttpPost]
    public async Task<IActionResult> Create(CreateCommecntModel model)
    {
        await _commentService.CreateAsync(model);

        if (_commentService.IsValid)
        {
            return Ok("Done");
        }

        _commentService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }
    [HttpGet]
    public async Task<IActionResult> Get(int lessonId)
    {
        var comments = await _commentService.GetByLessonAsync(lessonId);

        if (_commentService.IsValid)
        {
            return Ok(comments);
        }

        _commentService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }


    [HttpGet("comment-id")]
    public async Task<IActionResult> GetById(int id)
    {
        var comment = await _commentService.GetByIdAsync(id);

        if (_commentService.IsValid)
        {
            return Ok(comment);
        }

        _commentService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }
}