using FluentValidation;
using OnlineCourse.Application.Models.Review;

namespace OnlineCourse.Application.Validators.Review;

public class CreateReviewModelValidator : AbstractValidator<CreateReviewModel>
{
    public CreateReviewModelValidator()
    {
        RuleFor(x => x.UserID)
           .NotEmpty().WithMessage("User ID is required.");

        RuleFor(x => x.CourseId)
            .GreaterThan(0).WithMessage("Course ID must be greater than 0.");

        RuleFor(x => x.Comment)
            .NotEmpty().WithMessage("Comment is required.")
            .Length(3, 500).WithMessage("Comment must be between 3 and 500 characters long.");

        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5)
            .WithMessage("Rating must be between 1 and 5.");
    }
}
