using FluentValidation;
using OnlineCourse.Application.Models.User;

namespace OnlineCourse.Application.Validators.User;

public class RegisterModelValidator : AbstractValidator<RegisterModel>
{
    public RegisterModelValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty()
            .Length(3, 70)
            .Matches(@"^[A-Z][a-z]+(?:\s[A-Z][a-z]+)+$")
            .WithMessage("To'liq ism formatda bo'lishi kerak (Masalan: John Doe)");

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")
            .WithMessage("Email noto'g'ri formatda");

        RuleFor(x => x.UserName)
            .NotEmpty()
            .Matches(@"^(?![0-9])[a-zA-Z0-9](?!.*[_.]{2})[a-zA-Z0-9._]{2,19}$")
            .WithMessage("Username noto‘g‘ri formatda");

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6)
            .WithMessage("Parol bo‘sh bo‘lmasligi va kamida 6 ta belgidan iborat bo‘lishi kerak");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty()
            .Equal(x => x.Password)
            .WithMessage("Parollar mos emas");
    }
}
