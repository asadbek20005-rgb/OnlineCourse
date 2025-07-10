using OnlineCourse.Application.Dtos;
using OnlineCourse.Application.Models.Category;
using StatusGeneric;

namespace OnlineCourse.Application.ServiceContracts;

public interface ICategoryService : IStatusGeneric
{
    Task<CategoryDto?> GetByIdAsync(int categoryId);
    Task<IEnumerable<CategoryDto>> GetAllAsync();
    Task CreateAsync(CreateCategoryModel model);
    Task UpdateAsync(int categoryId, UpdateCategoryModel model);
    Task DeleteAsync(int categoryId);
    Task<int> GetTotalCourseAsync(GetTotalCourseModel model);


}
