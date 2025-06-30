using FluentValidation;
using OnlineCourse.Application.Models.User;

namespace OnlineCourse.Application.Validators.User;

public class RegisterModelValidator : AbstractValidator<RegisterModel>
{
    public RegisterModelValidator()
    {
        
    }
}
