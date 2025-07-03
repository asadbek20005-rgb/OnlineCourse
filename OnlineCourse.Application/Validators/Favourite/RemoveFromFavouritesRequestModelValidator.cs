using FluentValidation;
using OnlineCourse.Application.Models.Favourite;

namespace OnlineCourse.Application.Validators.Favourite;

public class RemoveFromFavouritesRequestModelValidator : AbstractValidator<RemoveFromFavouritesRequestModel>
{
    public RemoveFromFavouritesRequestModelValidator()
    {
        RuleFor(x => x.UserId)
           .NotEmpty().WithMessage("User ID is required.");

        RuleFor(x => x.CourseId)
            .GreaterThan(0).WithMessage("Course ID must be greater than 0.");
    }
}
