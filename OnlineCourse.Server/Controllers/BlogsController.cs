using Microsoft.AspNetCore.Mvc;
using OnlineCourse.Application.Extensions;
using OnlineCourse.Application.Models.Blog;
using OnlineCourse.Application.ServiceContracts;

namespace OnlineCourse.Server.Controllers;

[Route("api/v1/Users/user-id/[controller]")]
[ApiController]
public class BlogsController(IBlogService _blogService) : ControllerBase
{
    [HttpGet("/api/v1/Blogs")]
    public async Task<IActionResult> GetAll()
    {
        var blogs = await _blogService.GetAllAsync();

        if (_blogService.IsValid)
        {
            return Ok(blogs);
        }

        _blogService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }

    [HttpGet]
    public async Task<IActionResult> GetUserBlogs(Guid userId)
    {
        var blogs = await _blogService.GetBlogsByUserIdAsync(userId);

        if (_blogService.IsValid)
        {
            return Ok(blogs);
        }

        _blogService.CopyToModelState(ModelState);
        return BadRequest(ModelState);

    }


    [HttpPost]
    public async Task<IActionResult> Create(Guid userId, CreateBlogModel model)
    {
        await _blogService.CreateAsync(userId, model);

        if (_blogService.IsValid)
        {
            return Ok("Done");
        }

        _blogService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }

    [HttpPut("blog-id")]
    public async Task<IActionResult> Update(Guid userId, int blogId, UpdateBlogModel model)
    {
        await _blogService.UpdateAsync(userId, blogId, model);

        if (_blogService.IsValid)
        {
            return Ok("Done");
        }

        _blogService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }

    [HttpDelete("blog-id")]
    public async Task<IActionResult> Delete(Guid userId, int blogId)
    {
        await _blogService.DeleteAsync(userId, blogId);
        if (_blogService.IsValid)
        {
            return Ok("Done");
        }

        _blogService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }
}