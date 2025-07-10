using FluentValidation;
using OnlineCourse.Application.Models.Instructor;

namespace OnlineCourse.Application.Validators.Instructor;

public class UpdateInstructorModelValidator : AbstractValidator<UpdateInstructorModel>
{
    public UpdateInstructorModelValidator()
    {
        When(x => x.UserId.HasValue, () =>
        {
            RuleFor(x => x.UserId!.Value)
                .NotEmpty().WithMessage("User ID cannot be empty.");
        });

        When(x => !string.IsNullOrWhiteSpace(x.Bio), () =>
        {
            RuleFor(x => x.Bio!)
                .MinimumLength(10).WithMessage("Bio must be at least 10 characters long.")
                .MaximumLength(500).WithMessage("Bio must not exceed 500 characters.");
        });

        When(x => x.Experiance.HasValue, () =>
        {
            RuleFor(x => x.Experiance!.Value)
                .GreaterThan(0).WithMessage("Experience must be greater than 0.");
        });

       

    }
}
    