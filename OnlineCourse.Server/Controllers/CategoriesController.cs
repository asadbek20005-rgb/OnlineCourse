using Microsoft.AspNetCore.Mvc;
using OnlineCourse.Application.Extensions;
using OnlineCourse.Application.Models.Category;
using OnlineCourse.Application.ServiceContracts;

namespace OnlineCourse.Server.Controllers;


[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Asp.Versioning.ApiVersion("1.0")]
public class CategoriesController(ICategoryService categoryService) : ControllerBase
{
    private readonly ICategoryService _categoryService = categoryService;

    [HttpPost]
    public async Task<IActionResult> Create(CreateCategoryModel model)
    {
        await _categoryService.CreateAsync(model);
        if (_categoryService.IsValid)
        {
            return Ok("Done");
        }

        _categoryService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categries = await _categoryService.GetAllAsync();

        if (_categoryService.IsValid)
        {
            return Ok(categries);
        }

        _categoryService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }


    [HttpPut("category-id")]
    public async Task<IActionResult> Update(int categoryId, UpdateCategoryModel model)
    {
        await _categoryService.UpdateAsync(categoryId, model);

        if (_categoryService.IsValid)
        {
            return Ok("Done");
        }

        _categoryService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }


    [HttpDelete("category-id")]
    public async Task<IActionResult> Delete(int id)
    {
        await _categoryService.DeleteAsync(id);

        if (_categoryService.IsValid)
        {
            return Ok("Done");
        }

        _categoryService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }



  
}
