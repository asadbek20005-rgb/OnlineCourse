using FluentValidation;
using OnlineCourse.Application.Models.Instructor;

namespace OnlineCourse.Application.Validators.Instructor;

public class CreateInstructorModelValidator : AbstractValidator<CreateInstructorModel>
{
    public CreateInstructorModelValidator()
    {
        RuleFor(x => x.UserId)
           .NotEmpty().WithMessage("User ID is required.");

        When(x => !string.IsNullOrWhiteSpace(x.Bio), () =>
        {
            RuleFor(x => x.Bio!)
                .MinimumLength(10).WithMessage("Bio must be at least 10 characters long.")
                .MaximumLength(500).WithMessage("Bio must not exceed 500 characters.");
        });

        RuleFor(x => x.Experiance)
            .GreaterThan(0).WithMessage("Experience must be greater than 0.");

        RuleFor(x => x.ApprovedByAdmin)
            .Equal(false).WithMessage("Instructor cannot approve themselves.");
    }
}
