using FluentValidation;
using OnlineCourse.Application.Models.Lesson;

namespace OnlineCourse.Application.Validators.Lesson;

public class UpdateLessonModelValidator : AbstractValidator<UpdateLessonModel>
{
    public UpdateLessonModelValidator()
    {
       
        When(x => !string.IsNullOrWhiteSpace(x.Title), () =>
        {
            RuleFor(x => x.Title!)
                .Length(3, 50).WithMessage("Lesson title must be between 3 and 50 characters long.")
                .Matches(@"^(?!\s)[a-zA-Z0-9\s\-:',.&()]{3,50}(?<!\s)$")
                .WithMessage("Lesson title contains invalid characters or has leading/trailing spaces.");
        });
    }
}
