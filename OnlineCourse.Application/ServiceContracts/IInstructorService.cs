using OnlineCourse.Application.Dtos;
using OnlineCourse.Application.Models.Instructor;
using StatusGeneric;

namespace OnlineCourse.Application.ServiceContracts;

public interface IInstructorService : IStatusGeneric
{
    Task<InstructorDto?> GetInstructorByIdAsync(int instructorId);
    Task CreateInstructorAsync(CreateInstructorModel model);
    Task UpdateInstructorAsync(int instructorId, UpdateInstructorModel model);
    Task ApproveAsync(int instructorId);
    Task<IEnumerable<InstructorDto>> GetAllInstructorAsync();
    Task<IEnumerable<CourseDto>> GetAllCourseAsync(int instructorId);
    Task<int> GetCourseCountAsync(int instructorId);
    Task DeleteAsync(int id);

    Task<int> GetTotalInstructorsCountAsync();


}