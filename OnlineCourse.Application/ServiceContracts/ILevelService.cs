using OnlineCourse.Application.Dtos;
using OnlineCourse.Application.Models.Category;
using OnlineCourse.Application.Models.Level;
using StatusGeneric;

namespace OnlineCourse.Application.ServiceContracts;

public interface ILevelService : IStatusGeneric
{
    Task<LevelDto?> GetByIdAsync(int levelId);
    Task<IEnumerable<LevelDto>> GetAllAsync();
    Task CreateAsync(CreateLevelModel model);
    Task UpdateAsync(int levelId, UpdateLevelModel model);
    Task DeleteAsync(int levelId);
}
