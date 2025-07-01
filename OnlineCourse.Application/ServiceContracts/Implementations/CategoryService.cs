using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineCourse.Application.Dtos;
using OnlineCourse.Application.Models.Category;
using OnlineCourse.Application.RepositoryContracts;
using OnlineCourse.Domain.Entities;
using StatusGeneric;

namespace OnlineCourse.Application.ServiceContracts.Implementations;

public class CategoryService(
    IBaseRepositroy<Category> categoryRepository,
    IMapper mapper) : StatusGenericHandler, ICategoryService
{
    private readonly IBaseRepositroy<Category> _categoryRepository = categoryRepository;
    private readonly IMapper _mapper = mapper;

    public async Task CreateAsync(CreateCategoryModel model)
    {
        Category? category = await _categoryRepository.GetAll()
            .Where(x => x.Name.ToLower() == model.Name.ToLower())
            .FirstOrDefaultAsync();

        if (category is not null)
        {
            AddError($"Category with name: {model.Name} is already exist");
            return;
        }

        Category newCategory = _mapper.Map<Category>(model);

        await _categoryRepository.AddAsync(newCategory);
        await _categoryRepository.SaveChangesAsync();
    }

    public async Task DeleteAsync(int categoryId)
    {
        Category? category = await _categoryRepository.GetByIdAsync(categoryId);

        if (category is null)
        {
            AddError($"Category with id: {categoryId} is not found");
            return;
        }

        await _categoryRepository.DeleteAsync(category);
        await _categoryRepository.SaveChangesAsync();
    }

    public async Task<IEnumerable<CategoryDto>> GetAllAsync()
    {
        var categories = await _categoryRepository.GetAll()
            .ToListAsync();

        return _mapper.Map<List<CategoryDto>>(categories);
    }

    public async Task<CategoryDto?> GetByIdAsync(int categoryId)
    {
        Category? category = await _categoryRepository.GetByIdAsync(categoryId);

        if (category is null)
        {
            AddError($"Category with id: {categoryId} is not found");
            return null;
        }

        return _mapper.Map<CategoryDto>(category);
    }

    public async Task UpdateAsync(int categoryId, UpdateCategoryModel model)
    {
        Category? category = await _categoryRepository.GetByIdAsync(categoryId);

        if (category is null)
        {
            AddError($"Category with id: {categoryId} is not found");
            return;
        }


        Category updatedCategory = _mapper.Map(model, category);

        await _categoryRepository.UpdateAsync(updatedCategory);
        await _categoryRepository.SaveChangesAsync();
    }
}
