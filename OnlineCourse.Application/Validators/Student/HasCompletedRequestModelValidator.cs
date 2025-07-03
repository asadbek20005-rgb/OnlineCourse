using FluentValidation;
using OnlineCourse.Application.Models.Student;

namespace OnlineCourse.Application.Validators.Student;

public class HasCompletedRequestModelValidator : AbstractValidator<HasCompletedRequestModel>
{
    public HasCompletedRequestModelValidator()
    {
        RuleFor(x => x.StudentId)
         .GreaterThan(0).WithMessage("Student ID must be greater than 0.");

        RuleFor(x => x.CourseId)
            .GreaterThan(0).WithMessage("Course ID must be greater than 0.");
    }
}
