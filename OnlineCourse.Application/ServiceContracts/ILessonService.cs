using Microsoft.AspNetCore.Http;
using OnlineCourse.Application.Dtos;
using OnlineCourse.Application.Models.Course;
using OnlineCourse.Application.Models.Lesson;
using StatusGeneric;

namespace OnlineCourse.Application.ServiceContracts;

public interface ILessonService : IStatusGeneric
{
    Task<LessonDto?> GetByIdAsync(GetLessonByIdModel model);
    Task<IEnumerable<LessonDto>> GetByCourseAsync(int courseId);
    Task CreateAsync(CreateLessonModel model);
    Task UpdateAsync(GetLessonByIdModel model1, UpdateLessonModel model);
    Task DeleteAsync(DeleteLessonModel model);
    Task<bool> ExistAsync(int lessonId);
    Task<string> UploadVideoAsync(int lessonId, IFormFile file);
    Task<Stream> DownloadVideoAsync(DownloadVideoModel model);
    Task<int> GetLessonsCountByInstructorIdAsync(GetInstructorLessonsCount model);
}

//Task<ResultDto> ReorderAsync(int courseId, Dictionary<int, short> lessonOrders);
//Task<ResultDto<LessonDto>> GetNextLessonAsync(int courseId, int currentLessonOrder);
