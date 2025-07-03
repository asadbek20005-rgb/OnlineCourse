using FluentValidation;
using OnlineCourse.Application.Models.Favourite;

namespace OnlineCourse.Application.Validators.Favourite;

public class AddToFavouritesRequestModelValidator : AbstractValidator<AddToFavouriteRequestModel>
{
    public AddToFavouritesRequestModelValidator()
    {
        RuleFor(x => x.UserId)
           .NotEmpty().WithMessage("User ID is required.");

        RuleFor(x => x.CourseId)
            .GreaterThan(0).WithMessage("Course ID must be greater than 0.");
    }
}
