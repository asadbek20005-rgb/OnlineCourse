using FluentValidation;
using OnlineCourse.Application.Models.Lesson;

namespace OnlineCourse.Application.Validators.Lesson;

public class CreateLessonModelValidator : AbstractValidator<CreateLessonModel>
{
    public CreateLessonModelValidator()
    {
        RuleFor(x => x.CourseId)
            .GreaterThan(0).WithMessage("Course ID must be greater than 0.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Lesson title is required.")
            .Length(3, 50).WithMessage("Lesson title must be between 3 and 50 characters long.")
            .Matches(@"^(?!\s)[a-zA-Z0-9\s\-:',.&()]{3,50}(?<!\s)$")
            .WithMessage("Lesson title contains invalid characters or has leading/trailing spaces.");
    }
}
