using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OnlineCourse.Application.Dtos;
using OnlineCourse.Application.Models.Instructor;
using OnlineCourse.Application.RepositoryContracts;
using OnlineCourse.Domain.Entities;
using StatusGeneric;

namespace OnlineCourse.Application.ServiceContracts.Implementations;

public class InstructorService(
    IBaseRepositroy<Instructor> instructorRepository,
    IBaseRepositroy<User> userRepository,
    IMapper mapper,
    IValidator<CreateInstructorModel> createValidator,
    IValidator<UpdateInstructorModel> updateValidator) : StatusGenericHandler, IInstructorService
{
    private readonly IBaseRepositroy<Instructor> _instructorRepository = instructorRepository;
    private readonly IBaseRepositroy<User> _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IValidator<CreateInstructorModel> _createValidator = createValidator;
    private readonly IValidator<UpdateInstructorModel> _updateValidator = updateValidator;

    public async Task ApproveAsync(int instructorId)
    {
        Instructor? instructor = await _instructorRepository.GetByIdAsync(instructorId);
        if (instructor is null)
        {
            AddError($"Instructor with id: {instructorId} is not found");
            return;
        }

        instructor.ApprovedByAdmin = true;

        await _instructorRepository.UpdateAsync(instructor);
        await _instructorRepository.SaveChangesAsync();
    }

    public async Task CreateInstructorAsync(CreateInstructorModel model)
    {
        var validatorResult = await _createValidator.ValidateAsync(model);
        if (!validatorResult.IsValid)
        {
            foreach (var error in validatorResult.Errors)
            {
                AddError($"Validation error: {error.ErrorMessage}");
            }
            return;
        }
        User? user = await _userRepository.GetByIdAsync(model.UserId);
        if (user is null)
        {
            AddError($"User with id: {model.UserId} is not found");
            return;
        }

        bool instructorExist = await _instructorRepository.GetAll()
            .AnyAsync(x => x.UserId == model.UserId);

        if (instructorExist)
        {
            AddError($"Instructor with user id: {model.UserId} is already exist");
            return;
        }

        Instructor newInstructor = _mapper.Map<Instructor>(model);

        await _instructorRepository.AddAsync(newInstructor);
        await _instructorRepository.SaveChangesAsync();

    }

    public async Task DeleteAsync(int id)
    {
        Instructor? instructor = await _instructorRepository.GetByIdAsync(id);

        if (instructor is null)
        {
            AddError($"Instructor with id: {id} is not found");
            return;
        }

        await _instructorRepository.DeleteAsync(instructor);
        await _instructorRepository.SaveChangesAsync();
    }

    public async Task<IEnumerable<CourseDto>> GetAllCourseAsync(int instructorId)
    {
        Instructor? instructor = await _instructorRepository.GetByIdAsync(instructorId);
        if (instructor is null)
        {
            AddError($"Instructor with id: {instructorId} is not found");
            return Enumerable.Empty<CourseDto>();
        }
        return _mapper.Map<List<CourseDto>>(instructor.Courses);
    }

    public async Task<IEnumerable<InstructorDto>> GetAllInstructorAsync()
    {
        var instructors = await _instructorRepository.GetAll()
            .ToListAsync();

        return _mapper.Map<List<InstructorDto>>(instructors);
    }

    public async Task<int> GetCourseCountAsync(int instructorId)
    {
        Instructor? instructor = await _instructorRepository.GetByIdAsync(instructorId);
        if (instructor is null)
        {
            AddError($"Instructor with id: {instructorId} is not found");
            return 0;
        }
        return instructor.Courses.Count();
    }

    public async Task<InstructorDto?> GetInstructorByIdAsync(int instructorId)
    {
        Instructor? instructor = await _instructorRepository.GetByIdAsync(instructorId);
        if (instructor is null)
        {
            AddError($"Instructor with id: {instructorId} is not found");
            return null;
        }
        return _mapper.Map<InstructorDto>(instructor);
    }

    public async Task<int> GetTotalInstructorsCountAsync()
    {
        return await _instructorRepository.GetAll().CountAsync();
    }

    public async Task UpdateInstructorAsync(int instructorId, UpdateInstructorModel model)
    {
        var validatorResult = await _updateValidator.ValidateAsync(model);
        if (!validatorResult.IsValid)
        {
            foreach (var error in validatorResult.Errors)
            {
                AddError($"Validation error: {error.ErrorMessage}");
            }
        }
        Instructor? instructor = await _instructorRepository.GetByIdAsync(instructorId);
        if (instructor is null)
        {
            AddError($"Instructor with id: {instructorId} is not found");
            return;
        }
        Instructor updatedInstructor = _mapper.Map(model, instructor);
        await _instructorRepository.UpdateAsync(updatedInstructor);
        await _instructorRepository.SaveChangesAsync();
    }
}
