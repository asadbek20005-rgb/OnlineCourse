using FluentValidation;
using OnlineCourse.Application.Models.User;

namespace OnlineCourse.Application.Validators.User;

public class UpdateUserModelValidator : AbstractValidator<UpdateUserModel>
{
    public UpdateUserModelValidator()
    {
        When(x => !string.IsNullOrWhiteSpace(x.FullName), () =>
        {
            RuleFor(x => x.FullName!)
                .Length(3, 70).WithMessage("Full name must be between 3 and 70 characters.")
                .Matches(@"^[A-Z][a-z]+(?:\s[A-Z][a-z]+)+$")
                .WithMessage("Full name format is invalid. Example: John Doe");
        });

        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username is required.")
            .Matches(@"^(?![0-9])[a-zA-Z0-9](?!.*[_.]{2})[a-zA-Z0-9._]{2,19}$")
            .WithMessage("Invalid username format.");
    }
}
