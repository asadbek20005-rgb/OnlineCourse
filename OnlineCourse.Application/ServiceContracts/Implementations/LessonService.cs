using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
    IMinioService _minioService,
    IBaseRepositroy<Instructor> _instructorRepository) : StatusGenericHandler, ILessonService
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
    public async Task DeleteAsync(DeleteLessonModel model)
    {

        Course? course = await _courseRepository.GetByIdAsync(model.CourseId);

        if (course is null)
        {
            AddError($"Course with id: {model.CourseId} is not found");
            return;
        }


        Lesson? lesson = await _lessonRepository.GetAll()
                 .Where(x => x.CourseId == course.Id && x.Id == model.LessonId)
                 .FirstOrDefaultAsync();


        if (lesson is null)
        {
            AddError($"There is no lesson with id: {model.LessonId} for the course with id: {model.CourseId}");
            return;
        }

        await _lessonRepository.DeleteAsync(lesson);
        await _lessonRepository.SaveChangesAsync();
    }
    public async Task<Stream> DownloadVideoAsync(DownloadVideoModel model)
    {
        Lesson? lesson = await _lessonRepository.GetByIdAsync(model.LessonId);

        if (lesson is null)
        {
            AddError($"Lesson with id: {model.LessonId} is not found");
            return Stream.Null;
        }

        if (string.IsNullOrWhiteSpace(lesson.VideoUrl))
        {
            AddError($"There is no file name for the lesson with id: {model.LessonId}");
            return Stream.Null;
        }

        return await _minioService.DownloadFileAsync(lesson.VideoUrl);
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
    public async Task<LessonDto?> GetByIdAsync(GetLessonByIdModel model)
    {
        Course? course = await _courseRepository.GetByIdAsync(model.CourseId);

        if (course is null)
        {
            AddError($"Course with id: {model.CourseId} is not found");
            return null;
        }


        Lesson? lesson = await _lessonRepository.GetAll()
            .Where(x => x.CourseId == course.Id && x.Id == model.LessonId)
            .FirstOrDefaultAsync();


        if (lesson is null)
        {
            AddError($"There is no lesson with id: {model.LessonId} for the course with id: {model.CourseId}");
            return null;
        }

        return _mapper.Map<LessonDto>(lesson);
    }
    public async Task<int> GetLessonsCountByInstructorIdAsync(GetInstructorLessonsCount model)
    {
        Instructor? instructor = await _instructorRepository.GetByIdAsync(model.InstructorId);

        if (instructor is null)
        {
            AddError($"Instructor with id: {model.InstructorId} is not found");
            return 0;
        }

        var courses = instructor.Courses;

        return courses.Select(x => x.Lessons).Count();
    }
    public async Task UpdateAsync(GetLessonByIdModel model1, UpdateLessonModel model)
    {
        var validatorResult = await _updateLessonValidator.ValidateAsync(model);
        if (!validatorResult.IsValid)
        {
            foreach (var error in validatorResult.Errors)
            {
                AddError($"Validation error: {error.ErrorMessage}");
            }
        }


        Course? course = await _courseRepository.GetByIdAsync(model1.CourseId);
        if (course is null)
        {
            AddError($"Course with id: {model1.CourseId} is not found");
            return;
        }



        Lesson? lesson = await _lessonRepository.GetAll()
            .Where(x => x.CourseId == course.Id && x.Id == model1.LessonId)
            .FirstOrDefaultAsync();


        if (lesson is null)
        {
            AddError($"There is no lesson with id: {model1.LessonId} for the course with id: {model1.CourseId}");
            return;
        }


        Lesson? updateLesson = _mapper.Map(model, lesson);


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

        lesson.VideoUrl = fileName;

        await _lessonRepository.UpdateAsync(lesson);
        await _lessonRepository.SaveChangesAsync();

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