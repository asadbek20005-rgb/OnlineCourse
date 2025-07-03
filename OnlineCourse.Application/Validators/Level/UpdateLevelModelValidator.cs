using FluentValidation;
using OnlineCourse.Application.Models.Level;

namespace OnlineCourse.Application.Validators.Level;

public class UpdateLevelModelValidator : AbstractValidator<UpdateLevelModel>
{
    public UpdateLevelModelValidator()
    {
        When(x => !string.IsNullOrWhiteSpace(x.Name), () =>
        {
            RuleFor(x => x.Name!)
                .MinimumLength(2).WithMessage("Level name must be at least 2 characters long.")
                .MaximumLength(100).WithMessage("Level name must not exceed 100 characters.");
        });
    }
}
