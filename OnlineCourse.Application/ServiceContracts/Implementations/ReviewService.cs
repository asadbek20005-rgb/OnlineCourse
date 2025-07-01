using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineCourse.Application.Dtos;
using OnlineCourse.Application.Models.Review;
using OnlineCourse.Application.RepositoryContracts;
using OnlineCourse.Domain.Entities;
using StatusGeneric;

namespace OnlineCourse.Application.ServiceContracts.Implementations;

public class ReviewService(IBaseRepositroy<Review> reviewRepository,
    IMapper mapper) : StatusGenericHandler, IReviewService
{
    private readonly IBaseRepositroy<Review> _reviewRepository = reviewRepository;
    private readonly IMapper _mapper = mapper;
    public async Task CreateAsync(CreateReviewModel model)
    {
        Review? review = await _reviewRepository.GetAll()
                         .Where(x => x.UserID == model.UserID && x.CourseId == model.CourseId)
                         .SingleOrDefaultAsync();

        if (review is not null)
        {
            AddError($"User with id: {model.UserID} is already given reviews to this course with id {model.CourseId}");
            return;
        }

        Review newReview = _mapper.Map<Review>(model);
        newReview.HasReviewed = true;
        await _reviewRepository.AddAsync(newReview);
        await _reviewRepository.SaveChangesAsync();
    }

    public async Task DeleteAsync(int reviewId)
    {
        Review? review = await _reviewRepository.GetByIdAsync(reviewId);

        if (review is null)
        {
            AddError($"Review with id: {reviewId} is not found");
            return;
        }

        await _reviewRepository.DeleteAsync(review);
        await _reviewRepository.SaveChangesAsync();
    }

    public IEnumerable<CourseDto> GetByCourse(int courseId)
    {
        throw new NotImplementedException();
    }

    public async Task<ReviewDto?> GetByIdAsync(int reviewId)
    {
        Review? review = await _reviewRepository.GetByIdAsync(reviewId);

        if (review is null)
        {
            AddError($"Review with id: {reviewId} is not found");
            return null;
        }

        return _mapper.Map<ReviewDto>(review);

    }

    public async Task<bool?> HasReviewedAsync(HasReviewedRequest model)
    {
        Review? review = await _reviewRepository.GetAll()
                         .Where(x => x.UserID == model.UserId && x.CourseId == model.CourseId)
                         .SingleOrDefaultAsync();

        if (review is null)
        {
            AddError($"Review with user id: {model.UserId} and course id: {model.CourseId} is not found");
            return null;
        }

        return review.HasReviewed;
    }

    public async Task UpdateAsync(int reviewId, UpdateReviewModel model)
    {
        Review? review = await _reviewRepository.GetByIdAsync(reviewId);

        if (review is null)
        {
            AddError($"Review with id: {reviewId} is not found");
            return;
        }


        Review? updatedReview = _mapper.Map(model, review);

        await _reviewRepository.UpdateAsync(updatedReview);
        await _reviewRepository.SaveChangesAsync();
    }
}
