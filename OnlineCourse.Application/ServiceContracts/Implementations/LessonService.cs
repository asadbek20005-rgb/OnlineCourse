using AutoMapper;
using Microsoft.AspNetCore.Http;
using OnlineCourse.Application.Dtos;
using OnlineCourse.Application.Models.Course;
using OnlineCourse.Application.Models.Lesson;
using OnlineCourse.Application.RepositoryContracts;
using OnlineCourse.Domain.Entities;
using StatusGeneric;

namespace OnlineCourse.Application.ServiceContracts.Implementations;

public class LessonService(
    IBaseRepositroy<Lesson> lessonRepository,
    IBaseRepositroy<Course> courseRepository,
    IMapper mapper) : StatusGenericHandler, ILessonService
{
    private readonly IBaseRepositroy<Lesson> _lessonRepository = lessonRepository;
    private readonly IBaseRepositroy<Course> _courseRepository = courseRepository;
    private readonly IMapper _mapper = mapper;


    public async Task CreateAsync(CreateLessonModel model)
    {
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

    public async Task UpdateAsync(int lessonId, UpdateCourseModel model)
    {
        Lesson? lesson = await _lessonRepository.GetByIdAsync(lessonId);

        if (lesson is null)
        {
            AddError($"Lesson with id: {lessonId} is not found");
            return;
        }
    }

    public Task<string> UploadVideoAsync(int lessonId, IFormFile file)
    {
        throw new NotImplementedException();
    }
}