using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineCourse.Application.Dtos;
using OnlineCourse.Application.Models.Level;
using OnlineCourse.Application.RepositoryContracts;
using OnlineCourse.Domain.Entities;
using StatusGeneric;

namespace OnlineCourse.Application.ServiceContracts.Implementations;

public class LevelService(
    IBaseRepositroy<Level> levelRepository,
    IMapper mapper) : StatusGenericHandler, ILevelService
{
    private readonly IBaseRepositroy<Level> _levelRepository = levelRepository;
    private readonly IMapper _mapper = mapper;

    public async Task CreateAsync(CreateLevelModel model)
    {
        bool levelExist = await _levelRepository.GetAll()
            .AnyAsync(x => x.Name.ToLower() == model.Name.ToLower());

        if (levelExist)
        {
            AddError($"Level with name: {model.Name} is already exist");
            return;
        }

        Level? newLevel = _mapper.Map<Level>(model);

        await _levelRepository.AddAsync(newLevel);
        await _levelRepository.SaveChangesAsync();
    }

    public async Task DeleteAsync(int levelId)
    {
        Level? level = await _levelRepository.GetByIdAsync(levelId);

        if (level is null)
        {
            AddError($"Level with id: {levelId} is not found");
            return;
        }

        await _levelRepository.DeleteAsync(level);
        await _levelRepository.SaveChangesAsync();
    }

    public async Task<IEnumerable<LevelDto>> GetAllAsync()
    {
        var levels = await _levelRepository.GetAll().ToListAsync();

        return _mapper.Map<List<LevelDto>>(levels);

    }

    public async Task<LevelDto?> GetByIdAsync(int levelId)
    {
        Level? level = await _levelRepository.GetByIdAsync(levelId);

        if (level is null)
        {
            AddError($"Level with id: {levelId} is not found");
            return null;
        }

        return _mapper.Map<LevelDto>(level);

    }

    public async Task UpdateAsync(int levelId, UpdateLevelModel model)
    {
        Level? level = await _levelRepository.GetByIdAsync(levelId);

        if (level is null)
        {
            AddError($"Level with id: {levelId} is not found");
            return;
        }

        Level updatedLevel = _mapper.Map(model, level);

        await _levelRepository.UpdateAsync(updatedLevel);
        await _levelRepository.SaveChangesAsync();
    }
}
