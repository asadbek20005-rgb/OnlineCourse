using FluentValidation;
using OnlineCourse.Application.Models.User;

namespace OnlineCourse.Application.Validators.User;

public class LoginModelValidator : AbstractValidator<LoginModel>
{
    public LoginModelValidator()
    {
        RuleFor(x => x.Email)
           .NotEmpty().When(x => string.IsNullOrWhiteSpace(x.UserName))
           .WithMessage("Either Email or Username is required.")
           .EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email))
           .WithMessage("Invalid email format.");

        RuleFor(x => x.UserName)
            .NotEmpty().When(x => string.IsNullOrWhiteSpace(x.Email))
            .WithMessage("Either Username or Email is required.")
            .Matches(@"^(?![0-9])[a-zA-Z0-9](?!.*[_.]{2})[a-zA-Z0-9._]{2,19}$")
            .When(x => !string.IsNullOrWhiteSpace(x.UserName))
            .WithMessage("Invalid username format.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
    }
}
