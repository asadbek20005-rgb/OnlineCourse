using FluentValidation;
using OnlineCourse.Application.Models.Category;

namespace OnlineCourse.Application.Validators.Category;

public class UpdateCategoryModelValidator : AbstractValidator<UpdateCategoryModel>
{
    public UpdateCategoryModelValidator()
    {
        When(x => !string.IsNullOrWhiteSpace(x.Name), () =>
        {
            RuleFor(x => x.Name!)
                .MinimumLength(2).WithMessage("Category name must be at least 2 characters long.")
                .MaximumLength(100).WithMessage("Category name must not exceed 100 characters.");
        });
    }
}
