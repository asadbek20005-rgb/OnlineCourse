using OnlineCourse.Application.Dtos;
using OnlineCourse.Application.Models.Student;
using StatusGeneric;

namespace OnlineCourse.Application.ServiceContracts;

public interface IStudentService : IStatusGeneric
{
    Task<StudentDto?> GetStudentByIdAsync(int id);
    Task<IEnumerable<CourseDto>> GetEnrolledCourses(int id);
    Task EnrollAsync(EnrollRequestModel model);
    Task<StudentProgressDto?> GetProgressAsync(GetProgressRequestModel model);
    Task UpdateProgressAsync(UpdateProgressModel model);
    Task<bool?> HasCompletedCourseAsync(HasCompletedRequestModel model);
}