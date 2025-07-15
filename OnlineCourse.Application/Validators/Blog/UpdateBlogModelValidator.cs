using FluentValidation;
using OnlineCourse.Application.Models.Blog;

namespace OnlineCourse.Application.Validators.Blog;

public class UpdateBlogModelValidator : AbstractValidator<UpdateBlogModel>
{
    public UpdateBlogModelValidator()
    {
        RuleFor(x => x.Title)
          .MinimumLength(3).WithMessage("Sarlavha kamida 3 ta belgidan iborat bo‘lishi kerak.")
          .MaximumLength(100).WithMessage("Sarlavha 100 ta belgidan oshmasligi kerak.")
          .When(x => !string.IsNullOrWhiteSpace(x.Title));

        RuleFor(x => x.Details)
            .MinimumLength(10).WithMessage("Tafsilotlar kamida 10 ta belgidan iborat bo‘lishi kerak.")
            .MaximumLength(250).WithMessage("Tafsilotlar 250 ta belgidan oshmasligi kerak.")
            .When(x => !string.IsNullOrWhiteSpace(x.Details));

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId bo‘sh bo‘lmasligi kerak.")
            .When(x => x.UserId.HasValue);
    }
}
