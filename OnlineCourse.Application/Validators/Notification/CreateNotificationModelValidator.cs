using FluentValidation;
using OnlineCourse.Application.Models.Notification;

namespace OnlineCourse.Application.Validators.Notification;

public class CreateNotificationModelValidator : AbstractValidator<CreateNotificationModel>
{
    public CreateNotificationModelValidator()
    {
        RuleFor(x => x.UserID)
           .NotEmpty().WithMessage("User ID is required.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .Length(3, 50).WithMessage("Title must be between 3 and 50 characters long.")
            .Matches(@"^(?!\s)[a-zA-Z0-9\s\-:',.&()]{3,50}(?<!\s)$")
            .WithMessage("Title contains invalid characters or has leading/trailing spaces.");

        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("Message is required.")
            .Length(3, 50).WithMessage("Message must be between 3 and 50 characters long.");
    }
}
