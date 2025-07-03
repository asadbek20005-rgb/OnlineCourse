using FluentValidation;
using OnlineCourse.Application.Models.Level;

namespace OnlineCourse.Application.Validators.Level;

public class CreateLevelModelValidator : AbstractValidator<CreateLevelModel>
{
    public CreateLevelModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Level name is required.")
            .MinimumLength(2).WithMessage("Level name must be at least 2 characters long.")
            .MaximumLength(100).WithMessage("Level name must not exceed 100 characters.");
    }
}
