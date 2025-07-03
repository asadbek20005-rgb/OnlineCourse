using Microsoft.AspNetCore.Mvc;
using OnlineCourse.Application.Extensions;
using OnlineCourse.Application.Models.Level;
using OnlineCourse.Application.ServiceContracts;

namespace OnlineCourse.Server.Controllers;


[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Asp.Versioning.ApiVersion("1.0")]
public class LevelsController(ILevelService levelService) : ControllerBase
{
    private readonly ILevelService _levelService = levelService;

    [HttpPost]
    public async Task<IActionResult> Create(CreateLevelModel model)
    {
        await _levelService.CreateAsync(model);


        if (_levelService.IsValid)
        {
            return Ok("Done");
        }

        _levelService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var levels = await _levelService.GetAllAsync();
        if (_levelService.IsValid)
        {
            return Ok(levels);
        }

        _levelService.CopyToModelState(ModelState);
        return BadRequest(ModelState);

    }




    [HttpPut("level-id")]
    public async Task<IActionResult> Update(int levelId, UpdateLevelModel model)
    {
        await _levelService.UpdateAsync(levelId, model);

        if (_levelService.IsValid)
        {
            return Ok("Done");
        }

        _levelService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }



    [HttpDelete("level-id")]
    public async Task<IActionResult> Delete(int levelId)
    {
        await _levelService.DeleteAsync(levelId);

        if (_levelService.IsValid)
        {
            return Ok("Done");
        }

        _levelService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }

}
