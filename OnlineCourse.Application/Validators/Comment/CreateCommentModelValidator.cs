using FluentValidation;
using OnlineCourse.Application.Models.Comment;

namespace OnlineCourse.Application.Validators.Comment;

public class CreateCommentModelValidator : AbstractValidator<CreateCommecntModel>
{
    public CreateCommentModelValidator()
    {
        RuleFor(x => x.LessonId)
          .GreaterThan(0).WithMessage("Lesson ID must be a positive number.");

        RuleFor(x => x.UserID)
            .NotEmpty().WithMessage("User ID is required.");

        RuleFor(x => x.Text)
            .NotEmpty().WithMessage("Comment text is required.")
            .MinimumLength(3).WithMessage("Comment must be at least 3 characters long.");
    }
}