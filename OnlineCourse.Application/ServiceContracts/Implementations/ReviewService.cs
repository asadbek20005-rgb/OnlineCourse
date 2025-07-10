using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OnlineCourse.Application.Dtos;
using OnlineCourse.Application.Models.Review;
using OnlineCourse.Application.RepositoryContracts;
using OnlineCourse.Domain.Entities;
using StatusGeneric;

namespace OnlineCourse.Application.ServiceContracts.Implementations;

public class ReviewService(IBaseRepositroy<Review> reviewRepository,
    IBaseRepositroy<Course> _courseRepository,
    IMapper mapper,
    IValidator<CreateReviewModel> validator,
    IValidator<HasReviewedRequest> validator1,
    IValidator<UpdateReviewModel> validator2,
    IBaseRepositroy<User> _userRepository) : StatusGenericHandler, IReviewService
{
    private readonly IBaseRepositroy<Review> _reviewRepository = reviewRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IValidator<CreateReviewModel> _createReviewModelValidator = validator;
    private readonly IValidator<HasReviewedRequest> _hasReviewModelValidator = validator1;
    private readonly IValidator<UpdateReviewModel> _updateValidator = validator2;

    public async Task CreateAsync(CreateReviewModel model)
    {
        var validatorResult = await _createReviewModelValidator.ValidateAsync(model);
        if (!validatorResult.IsValid)
        {
            foreach (var error in validatorResult.Errors)
            {
                AddError($"Validation error: {error.ErrorMessage}");
            }
        }

        User? user = await _userRepository.GetByIdAsync(model.UserID);

        if (user is null)
        {
            AddError($"User with id: {model.UserID} is not found");
            return;
        }


        Course? course = await _courseRepository.GetByIdAsync(model.CourseId);
        if (course is null)
        {
            AddError($"Course with id: {model.CourseId} is not found");
            return;
        }


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

    public async Task<IEnumerable<ReviewDto>> GetByCourse(int courseId)
    {
        Course? course = await _courseRepository.GetByIdAsync(courseId);

        if (course is null)
        {
            AddError($"Course with id: {courseId} is not found");
            return Enumerable.Empty<ReviewDto>();
        }

        var reviews = await _reviewRepository.GetAll()
            .Where(x => x.CourseId == course.Id)
            .ToListAsync();

        return _mapper.Map<List<ReviewDto>>(reviews);

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
        var validatorResult = await _hasReviewModelValidator.ValidateAsync(model);
        if (!validatorResult.IsValid)
        {
            foreach (var error in validatorResult.Errors)
            {
                AddError($"Validation error: {error.ErrorMessage}");
            }
        }

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
        var validatorResult = await _updateValidator.ValidateAsync(model);
        if (!validatorResult.IsValid)
        {
            foreach (var error in validatorResult.Errors)
            {
                AddError($"Validation error: {error.ErrorMessage}");
            }
        }

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
