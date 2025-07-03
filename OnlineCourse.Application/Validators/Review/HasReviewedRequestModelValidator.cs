using FluentValidation;
using OnlineCourse.Application.Models.Review;

namespace OnlineCourse.Application.Validators.Review;

public  class HasReviewedRequestModelValidator : AbstractValidator<HasReviewedRequest>
{
    public HasReviewedRequestModelValidator()
    {
        RuleFor(x => x.UserId)
           .NotEmpty().WithMessage("User ID is required.");

        RuleFor(x => x.CourseId)
            .GreaterThan(0).WithMessage("Course ID must be greater than 0.");
    }
}
