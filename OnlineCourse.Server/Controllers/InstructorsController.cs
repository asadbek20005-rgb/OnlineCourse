using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using OnlineCourse.Application.Extensions;
using OnlineCourse.Application.Models.Instructor;
using OnlineCourse.Application.ServiceContracts;

namespace OnlineCourse.Server.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class InstructorsController(IInstructorService instructorService) : ControllerBase
{
    private readonly IInstructorService _instructorService = instructorService;

    [HttpPost]
    public async Task<IActionResult> Create(CreateInstructorModel model)
    {
        await _instructorService.CreateInstructorAsync(model);
        if (_instructorService.IsValid)
        {
            return Ok("Done");
        }

        _instructorService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }


    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var instructors = await _instructorService.GetAllInstructorAsync();
        if (_instructorService.IsValid)
        {
            return Ok(instructors);
        }
        _instructorService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetAll(int id)
    {
        var instructor = await _instructorService.GetInstructorByIdAsync(id);
        if (_instructorService.IsValid)
        {
            return Ok(instructor);
        }
        _instructorService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }


    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateInstructorModel model)
    {
        await _instructorService.UpdateInstructorAsync(id, model);
        if (_instructorService.IsValid)
        {
            return Ok("Done");
        }
        _instructorService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        return Ok("soon...");
    }



}
