using OnlineCourse.Application.Dtos;
using OnlineCourse.Application.Models.Review;
using StatusGeneric;

namespace OnlineCourse.Application.ServiceContracts;

public interface IReviewService : IStatusGeneric
{
    Task<ReviewDto?> GetByIdAsync(int reviewId);
    Task<IEnumerable<CourseDto>> GetByCourse(int courseId);
    Task CreateAsync(CreateReviewModel model);
    Task UpdateAsync(int reviewId, UpdateReviewModel model);
    Task DeleteAsync(int reviewId);
    Task<bool?> HasReviewedAsync(HasReviewedRequest model);
}
