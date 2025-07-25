using OnlineCourse.Application.Dtos;
using OnlineCourse.Application.Models.Category;
using OnlineCourse.Application.Models.Course;
using OnlineCourse.Application.Models.Pagination;
using StatusGeneric;

namespace OnlineCourse.Application.ServiceContracts;

public interface ICourseService : IStatusGeneric
{
    Task<CourseDto?> GetCourseByIdAsync(int courseId);
    Task<IEnumerable<CourseDto>> GetAllCourseAsync();
    Task<IEnumerable<CourseDto>> GetPublishedCoursesAsync();
    Task<IEnumerable<CourseDto>> GetCoursesByInstructorIdAsync(int instructorId);
    Task<IEnumerable<CourseDto>> GetCoursesByCategoryAsync(int categoryId);
    Task<IEnumerable<CourseDto>> SearchAsync(string query);
    Task<IEnumerable<CourseDto>> GetCoursesTopRatedAsync(int count);
    Task CreateAsync(CreateCourseModel model);
    Task UpdateAsync(int courseId, UpdateCourseModel model);
    Task DeleteAsync(int courseId);
    Task PublishAsync(PublishModel model);
    Task UpdateRatingAsync(int courseId);
    Task<IEnumerable<CourseDto>> GetCoursesByPagination(PaginationModel model);
    Task UploadImg(UploadCourseImgModel model);
    Task UnPublishAsync(UnPublishModel model);
    Task<decimal?> GetCourseRating(GetCourseRatingModel model);
    Task ApproveAsync(ApproveCourseModel model);
    Task RejectAsync(RejectCourseModel model);
    Task<IEnumerable<CourseDto>> GetCoursesByPriceAsync(GetCoursesByPriceModel model);
    Task<IEnumerable<CourseDto>> GetCoursesByLevelAsync(GetCoursesByLevelModel model);
    Task<int> GetTotalStudentInCourse(GetTotalStudentModel model);
    Task<int> GetTotalCourseCountAsync();
    Task<int> GetCourseCountByCategoryIdAsync(GetCourseCountByCategoryIdModel model);
    Task<IEnumerable<CourseDto>> GetStudenFavoriteCoursesAsync(GetStudentFavoriteCoursesModel model);
}