using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OnlineCourse.Application.Dtos;
using OnlineCourse.Application.Models.Category;
using OnlineCourse.Application.RepositoryContracts;
using OnlineCourse.Domain.Entities;
using StatusGeneric;

namespace OnlineCourse.Application.ServiceContracts.Implementations;

public class CategoryService(
    IBaseRepositroy<Category> categoryRepository,
    IMapper mapper,
    IValidator<CreateCategoryModel> createValidator,
    IValidator<UpdateCategoryModel> updateValidator,
    IBaseRepositroy<Course> _courseRepository) : StatusGenericHandler, ICategoryService
{
    private readonly IBaseRepositroy<Category> _categoryRepository = categoryRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IValidator<CreateCategoryModel> _createValidator = createValidator;
    private readonly IValidator<UpdateCategoryModel> _updateValidator = updateValidator;

    public async Task CreateAsync(CreateCategoryModel model)
    {
        var validatorResult = await _createValidator.ValidateAsync(model);
        if (!validatorResult.IsValid)
        {
            foreach (var error in validatorResult.Errors)
            {
                AddError($"Validation error: {error.ErrorMessage}");
            }
        }
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

    public async Task<int> GetTotalCourseAsync(GetTotalCourseModel model)
    {
        Category? category = await _categoryRepository.GetByIdAsync(model.CategoryId);

        if (category is null)
        {
            AddError($"Category with id: {model.CategoryId} is not found");
            return 0;
        }

        int count = await _courseRepository.GetAll()
            .Where(x => x.CategoryId == category.Id)
            .CountAsync();

        return count;
    }

    public async Task UpdateAsync(int categoryId, UpdateCategoryModel model)
    {
        var validatorResult = await _updateValidator.ValidateAsync(model);
        if (!validatorResult.IsValid)
        {
            foreach (var error in validatorResult.Errors)
            {
                AddError($"Validation error: {error.ErrorMessage}");
            }
        }
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
