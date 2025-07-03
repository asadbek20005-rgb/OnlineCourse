using FluentValidation;
using OnlineCourse.Application.Models.Student;

namespace OnlineCourse.Application.Validators.Student;

public class UpdateProgressModelValidator : AbstractValidator<UpdateProgressModel>
{
    public UpdateProgressModelValidator()
    {
        When(x => x.StudentId.HasValue, () =>
        {
            RuleFor(x => x.StudentId!.Value)
                .GreaterThan(0).WithMessage("Student ID must be greater than 0.");
        });

        When(x => x.CourseId.HasValue, () =>
        {
            RuleFor(x => x.CourseId!.Value)
                .GreaterThan(0).WithMessage("Course ID must be greater than 0.");
        });

        When(x => x.LessonId.HasValue, () =>
        {
            RuleFor(x => x.LessonId!.Value)
                .GreaterThan(0).WithMessage("Lesson ID must be greater than 0.");
        });

        When(x => x.ProgressPercent.HasValue, () =>
        {
            RuleFor(x => x.ProgressPercent!.Value)
                .InclusiveBetween(0f, 100f)
                .WithMessage("Progress percent must be between 0 and 100.");
        });
    }
}
