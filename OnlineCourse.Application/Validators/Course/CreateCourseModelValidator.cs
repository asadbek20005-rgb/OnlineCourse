using FluentValidation;
using OnlineCourse.Application.Models.Course;

namespace OnlineCourse.Application.Validators.Course;

public class CreateCourseModelValidator : AbstractValidator<CreateCourseModel>
{
    public CreateCourseModelValidator()
    {
        RuleFor(x => x.InstructorId)
           .GreaterThan(0).WithMessage("Instructor ID must be greater than 0.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Course title is required.")
            .Length(3, 50).WithMessage("Course title must be between 3 and 50 characters long.")
            .Matches(@"^(?!\s)[a-zA-Z0-9\s\-:',.&()]{3,50}(?<!\s)$")
            .WithMessage("Course title contains invalid characters or leading/trailing whitespace.");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("Category ID must be greater than 0.");

        RuleFor(x => x.LevelId)
            .GreaterThan(0).WithMessage("Level ID must be greater than 0.");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).WithMessage("Price must be zero or a positive number.");
    }
}
