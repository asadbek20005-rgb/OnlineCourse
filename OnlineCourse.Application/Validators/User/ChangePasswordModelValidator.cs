using FluentValidation;
using OnlineCourse.Application.Models.User;

namespace OnlineCourse.Application.Validators.User;

public class ChangePasswordModelValidator : AbstractValidator<ChangePasswordModel>
{
    public ChangePasswordModelValidator()
    {
        RuleFor(x => x.Email)
           .NotEmpty().When(x => string.IsNullOrWhiteSpace(x.UserName))
           .WithMessage("Either Email or Username is required.")
           .EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email))
           .WithMessage("Email format is invalid.");

        RuleFor(x => x.UserName)
            .NotEmpty().When(x => string.IsNullOrWhiteSpace(x.Email))
            .WithMessage("Either Username or Email is required.")
            .MinimumLength(3).When(x => !string.IsNullOrWhiteSpace(x.UserName))
            .WithMessage("Username must be at least 3 characters long.");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("New password is required.")
            .MinimumLength(6).WithMessage("New password must be at least 6 characters.")
            .Matches(@"[A-Z]").WithMessage("New password must contain at least one uppercase letter.")
            .Matches(@"[a-z]").WithMessage("New password must contain at least one lowercase letter.")
            .Matches(@"\d").WithMessage("New password must contain at least one number.")
            .Matches(@"[\W_]").WithMessage("New password must contain at least one special character.");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("Confirm password is required.")
            .Equal(x => x.NewPassword).WithMessage("Passwords do not match.");
    }
}
