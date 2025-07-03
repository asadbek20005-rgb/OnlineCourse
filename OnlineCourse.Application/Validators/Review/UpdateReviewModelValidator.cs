using FluentValidation;
using OnlineCourse.Application.Models.Review;

namespace OnlineCourse.Application.Validators.Review;

public class UpdateReviewModelValidator : AbstractValidator<UpdateReviewModel>
{
    public UpdateReviewModelValidator()
    {
        When(x => x.UserID.HasValue, () =>
        {
            RuleFor(x => x.UserID!.Value)
                .NotEmpty().WithMessage("User ID cannot be empty.");
        });

        When(x => x.CourseId.HasValue, () =>
        {
            RuleFor(x => x.CourseId!.Value)
                .GreaterThan(0).WithMessage("Course ID must be greater than 0.");
        });

        When(x => !string.IsNullOrWhiteSpace(x.Comment), () =>
        {
            RuleFor(x => x.Comment!)
                .Length(3, 500)
                .WithMessage("Comment must be between 3 and 500 characters.");
        });

        When(x => x.Rating.HasValue, () =>
        {
            RuleFor(x => x.Rating!.Value)
                .InclusiveBetween(1, 5)
                .WithMessage("Rating must be between 1 and 5.");
        });
    }
}

