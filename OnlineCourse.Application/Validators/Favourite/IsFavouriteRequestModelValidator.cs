using FluentValidation;
using OnlineCourse.Application.Models.Favourite;

namespace OnlineCourse.Application.Validators.Favourite;

public class IsFavouriteRequestModelValidator : AbstractValidator<IsFavouriteRequestModel>
{
    public IsFavouriteRequestModelValidator()
    {
        RuleFor(x => x.UserId)
           .NotEmpty().WithMessage("User ID is required.");

        RuleFor(x => x.CourseId)
            .GreaterThan(0).WithMessage("Course ID must be greater than 0.");
    }
}
