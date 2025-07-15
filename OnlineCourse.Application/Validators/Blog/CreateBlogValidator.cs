using FluentValidation;
using OnlineCourse.Application.Models.Blog;

namespace OnlineCourse.Application.Validators.Blog;

public class CreateBlogValidator : AbstractValidator<CreateBlogModel>
{
    public CreateBlogValidator()
    {
        RuleFor(x => x.Title)
          .NotEmpty().WithMessage("Sarlavha bo‘sh bo‘lishi mumkin emas.")
          .MinimumLength(3).WithMessage("Sarlavha kamida 3 ta belgidan iborat bo‘lishi kerak.")
          .MaximumLength(100).WithMessage("Sarlavha 100 ta belgidan oshmasligi kerak.");

        RuleFor(x => x.Details)
            .MinimumLength(10).When(x => !string.IsNullOrEmpty(x.Details))
            .WithMessage("Tafsilotlar kamida 10 ta belgidan iborat bo‘lishi kerak.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("Foydalanuvchi ID si bo‘sh bo‘lmasligi kerak.");
    }
}
