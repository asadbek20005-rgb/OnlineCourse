using FluentValidation;
using OnlineCourse.Application.Models.Payment;

namespace OnlineCourse.Application.Validators.Payment;

public class CreatePaymentModelValidator : AbstractValidator<CreatePaymentModel>
{
    public CreatePaymentModelValidator()
    {
        RuleFor(x => x.UserID)
           .NotEmpty().WithMessage("User ID is required.");

        RuleFor(x => x.CourseId)
            .GreaterThan(0).WithMessage("Course ID must be greater than 0.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Payment amount must be greater than 0.")
            .LessThanOrEqualTo(10_000_000).WithMessage("Payment amount is too large.");

        RuleFor(x => x.PaymentDate)
            .Must(BeAValidDate).WithMessage("Payment date must be a valid past or present date.")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Payment date cannot be in the future.");
    }

    private bool BeAValidDate(DateTime date)
    {
        return date != default;
    }
}
