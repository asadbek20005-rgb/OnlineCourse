using FluentValidation;
using OnlineCourse.Application.Models.Payment;

namespace OnlineCourse.Application.Validators.Payment;

public class HasPaidRequestModelValidator : AbstractValidator<HasPaidRequestModel>
{
    public HasPaidRequestModelValidator()
    {
        RuleFor(x => x.UserId)
               .NotEmpty().WithMessage("User ID is required.");

        RuleFor(x => x.CourseId)
            .GreaterThan(0).WithMessage("Course ID must be greater than 0.");
    }
}
