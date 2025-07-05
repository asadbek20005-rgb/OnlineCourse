using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OnlineCourse.Application.Dtos;
using OnlineCourse.Application.Models.Course;
using OnlineCourse.Application.Models.Minio;
using OnlineCourse.Application.Models.Pagination;
using OnlineCourse.Application.RepositoryContracts;
using OnlineCourse.Domain.Entities;
using StatusGeneric;

namespace OnlineCourse.Application.ServiceContracts.Implementations;

public class CourseService(
    IBaseRepositroy<Course> courseRepository,
    IBaseRepositroy<Instructor> instructorRepository,
    IBaseRepositroy<Level> levelRepository,
    IBaseRepositroy<Category> categoryRepository,
    IMapper mapper,
    IValidator<CreateCourseModel> createValidator,
    IValidator<UpdateCourseModel> updateValidator,
    IBaseRepositroy<User> _userRepository,
    IMinioService _minioService) : StatusGenericHandler, ICourseService
{
    private readonly IBaseRepositroy<Course> _courseRepository = courseRepository;
    private readonly IBaseRepositroy<Instructor> _instructorRepositroy = instructorRepository;
    private readonly IBaseRepositroy<Level> _levelRepository = levelRepository;
    private readonly IBaseRepositroy<Category> _categoryRepository = categoryRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IValidator<CreateCourseModel> _createValidator = createValidator;
    private readonly IValidator<UpdateCourseModel> _updateValidator = updateValidator;
    public async Task CreateAsync(CreateCourseModel model)
    {
        var validatorResult = await _createValidator.ValidateAsync(model);
        if (!validatorResult.IsValid)
        {
            foreach (var error in validatorResult.Errors)
            {
                AddError($"Validation error: {error.ErrorMessage}");
            }
        }
        Instructor? instructor = await _instructorRepositroy.GetByIdAsync(model.InstructorId);

        if (instructor is null)
        {
            AddError($"Instructor with id: {model.InstructorId} is not found");
            return;
        }

        Level? level = await _levelRepository.GetByIdAsync(model.LevelId);

        if (level is null)
        {
            AddError($"Level with id: {model.LevelId} is not found");
            return;
        }

        Category? category = await _categoryRepository.GetByIdAsync(model.CategoryId);

        if (category is null)
        {
            AddError($"Category with id: {model.CategoryId} is not found");
            return;
        }
        Course newCourse = _mapper.Map<Course>(model);
        await _courseRepository.AddAsync(newCourse);
        await _courseRepository.SaveChangesAsync();

    }

    public async Task DeleteAsync(int courseId)
    {
        Course? course = await _courseRepository.GetByIdAsync(courseId);
        if (course is null)
        {
            AddError($"Course with id: {courseId} is not found");
            return;
        }

        await _courseRepository.DeleteAsync(course);
        await _courseRepository.SaveChangesAsync();
    }

    public async Task<IEnumerable<CourseDto>> GetAllCourseAsync()
    {
        var courses = await _courseRepository.GetAll().ToListAsync();

        return _mapper.Map<List<CourseDto>>(courses);
    }

    public async Task<CourseDto?> GetCourseByIdAsync(int courseId)
    {
        Course? course = await _courseRepository.GetByIdAsync(courseId);
        if (course is null)
        {
            AddError($"Course with id: {courseId} is not found");
            return null;
        }
        return _mapper.Map<CourseDto>(course);
    }

    public async Task<IEnumerable<CourseDto>> GetCoursesByCategoryAsync(int categoryId)
    {
        Category? category = await _categoryRepository.GetByIdAsync(categoryId);

        if (category is null)
        {
            AddError($"Category with id: {categoryId} is not found");
            return Enumerable.Empty<CourseDto>();
        }

        var courses = await _courseRepository.GetAll()
            .Where(x => x.CategoryId == category.Id)
            .ToListAsync();

        return _mapper.Map<List<CourseDto>>(courses);
    }

    public async Task<IEnumerable<CourseDto>> GetCoursesByInstructorIdAsync(int instructorId)
    {
        Instructor? instructor = await _instructorRepositroy.GetByIdAsync(instructorId);

        if (instructor is null)
        {
            AddError($"Instructor with id: {instructorId} is not found");
            return Enumerable.Empty<CourseDto>();
        }
        var courses = await _courseRepository.GetAll()
            .Where(x => x.CategoryId == instructor.Id)
            .ToListAsync();

        return _mapper.Map<List<CourseDto>>(courses);
    }

    public async Task<IEnumerable<CourseDto>> GetCoursesByPagination(PaginationModel model)
    {
        var query = _courseRepository.GetAll();

        var courses = await query.Skip((model.Page - 1) * model.PageSize).Take(model.PageSize).ToListAsync();

        return _mapper.Map<List<CourseDto>>(courses);
    }

    public async Task<IEnumerable<CourseDto>> GetCoursesTopRatedAsync(int count)
    {
        var courses = await _courseRepository.GetAll()
            .OrderByDescending(x => x.Rating)
            .Take(10)
            .ToListAsync();

        return _mapper.Map<List<CourseDto>>(courses);
    }

    public async Task<IEnumerable<CourseDto>> GetPublishedCoursesAsync()
    {
        var courses = await _courseRepository.GetAll()
           .Where(x => x.IsPublished == true)
           .ToListAsync();

        return _mapper.Map<List<CourseDto>>(courses);

    }

    public async Task PublishAsync(PublishModel model)
    {
        User? user = await _userRepository.GetByIdAsync(model.UserId);

        if (user is null)
        {
            AddError($"User with id: {model.UserId} is not found");
            return;
        }

        Course? course = await _courseRepository.GetByIdAsync(model.CourseId);

        if (course is null)
        {
            AddError($"Course with id: {model.CourseId} is not found");
            return;
        }
        course.IsPublished = true;
        await _courseRepository.UpdateAsync(course);
        await _courseRepository.SaveChangesAsync();
    }

    public async Task<IEnumerable<CourseDto>> SearchAsync(string query)
    {
        var courses = await _courseRepository.GetAll()
            .Where(x => x.Title.ToLower().Contains(query.ToLower()))
            .ToListAsync();

        return _mapper.Map<List<CourseDto>>(courses);
    }

    public async Task UnPublishAsync(UnPublishModel model)
    {
        User? user = await _userRepository.GetByIdAsync(model.UserId);

        if (user is null)
        {
            AddError($"User with id: {model.UserId} is not found");
            return;
        }

        Course? course = await _courseRepository.GetByIdAsync(model.CourseId);

        if (course is null)
        {
            AddError($"Course with id: {model.CourseId} is not found");
            return;
        }

        course.IsPublished = false;
        await _courseRepository.UpdateAsync(course);
        await _courseRepository.SaveChangesAsync();
    }

    public async Task UpdateAsync(int courseId, UpdateCourseModel model)
    {
        var validatorResult = await _updateValidator.ValidateAsync(model);
        if (!validatorResult.IsValid)
        {
            foreach (var error in validatorResult.Errors)
            {
                AddError($"Validation error: {error.ErrorMessage}");
            }
        }
        Course? course = await _courseRepository.GetByIdAsync(courseId);
        if (course is null)
        {
            AddError($"Course with id: {courseId} is not found");
            return;
        }

        Category? category = await _categoryRepository.GetByIdAsync(model.CategoryId);

        if (category is null)
        {
            AddError($"Category with id: {model.CategoryId} is not found");
            return;
        }

        Level? level = await _levelRepository.GetByIdAsync(model.LevelId);

        if (level is null)
        {
            AddError($"Level with id: {model.LevelId} is not found");
            return;
        }

        Course updatedCourse = _mapper.Map(model, course);

        await _courseRepository.UpdateAsync(course);
        await _courseRepository.SaveChangesAsync();
    }

    public async Task UpdateRatingAsync(int courseId)
    {
        Course? course = await _courseRepository.GetByIdAsync(courseId);
        if (course is null)
        {
            AddError($"Course with id: {courseId} is not found");
            return;
        }
    }

    public async Task UploadImg(UploadCourseImgModel model)
    {
        User? user = await _userRepository.GetByIdAsync(model.UserId);

        if (user is null)
        {
            AddError($"User with id: {model.UserId} is not found");
            return;
        }

        Course? course = await _courseRepository.GetByIdAsync(model.CourseId);

        if (course is null)
        {
            AddError($"Course with id: {model.CourseId} is not found");
            return;
        }

        var (fileName, contentType, size, data) = await SaveFileDetails(model.File);

        var uploadFileModel = new UploadFileModel
        {
            FileName = fileName,
            ContentType = contentType,
            Size = size,
            Data = data
        };

        await _minioService.UploadFileAsync(uploadFileModel);

    }

    private async Task<(string FileName, string ContentType, long Size, MemoryStream Data)> SaveFileDetails(IFormFile file)
    {
        var fileName = Guid.NewGuid().ToString();
        string contentType = file.ContentType;
        long size = file.Length;

        var data = new MemoryStream();
        await file.CopyToAsync(data);

        return (fileName, contentType, size, data);
    }




}
