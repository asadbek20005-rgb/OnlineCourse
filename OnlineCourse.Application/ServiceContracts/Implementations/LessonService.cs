using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using OnlineCourse.Application.Dtos;
using OnlineCourse.Application.Models.Lesson;
using OnlineCourse.Application.Models.Minio;
using OnlineCourse.Application.RepositoryContracts;
using OnlineCourse.Domain.Entities;
using StatusGeneric;

namespace OnlineCourse.Application.ServiceContracts.Implementations;

public class LessonService(
    IBaseRepositroy<Lesson> lessonRepository,
    IBaseRepositroy<Course> courseRepository,
    IMapper mapper,
    IValidator<CreateLessonModel> createValidator,
    IValidator<UpdateLessonModel> updateValidator,
    IMinioService _minioService) : StatusGenericHandler, ILessonService
{
    private readonly IBaseRepositroy<Lesson> _lessonRepository = lessonRepository;
    private readonly IBaseRepositroy<Course> _courseRepository = courseRepository;
    private readonly IValidator<CreateLessonModel> _createLessonValidator = createValidator;
    private readonly IValidator<UpdateLessonModel> _updateLessonValidator = updateValidator;
    private readonly IMapper _mapper = mapper;


    public async Task CreateAsync(CreateLessonModel model)
    {
        var validatorResult = await _createLessonValidator.ValidateAsync(model);
        if (!validatorResult.IsValid)
        {
            foreach (var error in validatorResult.Errors)
            {
                AddError($"Validation error: {error.ErrorMessage}");
            }
        }
        Course? course = await _courseRepository.GetByIdAsync(model.CourseId);

        if (course is null)
        {
            AddError($"Course with id: {model.CourseId} is not found");
            return;
        }


        Lesson newLesson = _mapper.Map<Lesson>(model);

        await _lessonRepository.AddAsync(newLesson);
        await _lessonRepository.SaveChangesAsync();
    }

    public async Task DeleteAsync(int lessonId)
    {
        Lesson? lesson = await _lessonRepository.GetByIdAsync(lessonId);

        if (lesson is null)
        {
            AddError($"Lesson with id: {lessonId} is not found");
            return;
        }

        await _lessonRepository.DeleteAsync(lesson);
        await _lessonRepository.SaveChangesAsync();
    }

    public async Task<Stream> DownloadVideoAsync(int lessonId, string fileName)
    {
        Lesson? lesson = await _lessonRepository.GetByIdAsync(lessonId);

        if (lesson is null)
        {
            AddError($"Lesson with id: {lessonId} is not found");
            return Stream.Null;
        }

        return await _minioService.DownloadFileAsync(fileName);
    }

    public async Task<bool> ExistAsync(int lessonId)
    {
        Lesson? lesson = await _lessonRepository.GetByIdAsync(lessonId);

        if (lesson is null)
        {
            AddError($"Lesson with id: {lessonId} is not found");
            return false;
        }

        return true;
    }

    public async Task<IEnumerable<LessonDto>> GetByCourseAsync(int courseId)
    {
        Course? course = await _courseRepository.GetByIdAsync(courseId);

        if (course is null)
        {
            AddError($"Course with id: {courseId} is not found");
            return Enumerable.Empty<LessonDto>();
        }
        var courseLesson = _mapper.Map<List<LessonDto>>(course.Lessons);
        return courseLesson;
    }

    public async Task<LessonDto?> GetByIdAsync(int lessonId)
    {
        Lesson? lesson = await _lessonRepository.GetByIdAsync(lessonId);

        if (lesson is null)
        {
            AddError($"Lesson with id: {lessonId} is not found");
            return null;
        }

        return _mapper.Map<LessonDto>(lesson);
    }

    public async Task UpdateAsync(int lessonId, UpdateLessonModel model)
    {
        var validatorResult = await _updateLessonValidator.ValidateAsync(model);
        if (!validatorResult.IsValid)
        {
            foreach (var error in validatorResult.Errors)
            {
                AddError($"Validation error: {error.ErrorMessage}");
            }
        }
        Lesson? lesson = await _lessonRepository.GetByIdAsync(lessonId);

        if (lesson is null)
        {
            AddError($"Lesson with id: {lessonId} is not found");
            return;
        }
    }

    public async Task<string> UploadVideoAsync(int lessonId, IFormFile file)
    {
        Lesson? lesson = await _lessonRepository.GetByIdAsync(lessonId);

        if (lesson is null)
        {
            AddError($"Lesson with id: {lessonId} is not found");
            return string.Empty;
        }



        var (fileName, contentType, size, data) = await SaveFileDetails(file);

        var uploadFileModel = new UploadFileModel
        {
            FileName = fileName,
            ContentType = contentType,
            Size = size,
            Data = data
        };

        await _minioService.UploadFileAsync(uploadFileModel);
        return fileName;
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