using FluentValidation;
using OnlineCourse.Application.Models.Course;

namespace OnlineCourse.Application.Validators.Course;

public class UpdateCourseModelValidator : AbstractValidator<UpdateCourseModel>
{
    public UpdateCourseModelValidator()
    {

        When(x => !string.IsNullOrWhiteSpace(x.Title), () =>
        {
            RuleFor(x => x.Title!)
                .Length(3, 50).WithMessage("Course title must be between 3 and 50 characters long.")
                .Matches(@"^(?!\s)[a-zA-Z0-9\s\-:',.&()]{3,50}(?<!\s)$")
                .WithMessage("Course title contains invalid characters or leading/trailing whitespace.");
        });

        When(x => x.CategoryId.HasValue, () =>
        {
            RuleFor(x => x.CategoryId!.Value)
                .GreaterThan(0).WithMessage("Category ID must be greater than 0.");
        });

        When(x => x.LevelId.HasValue, () =>
        {
            RuleFor(x => x.LevelId!.Value)
                .GreaterThan(0).WithMessage("Level ID must be greater than 0.");
        });

        When(x => x.Price.HasValue, () =>
        {
            RuleFor(x => x.Price!.Value)
                .GreaterThanOrEqualTo(0).WithMessage("Price must be zero or a positive number.");
        });
    }
}
