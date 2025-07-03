namespace OnlineCourse.Application.Validators.Category;

using FluentValidation;
using global::OnlineCourse.Application.Models.Category;

public class CreateCategoryModelValidator : AbstractValidator<CreateCategoryModel>
{
    public CreateCategoryModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Category name is required.")
            .MinimumLength(2).WithMessage("Category name must be at least 2 characters long.")
            .MaximumLength(100).WithMessage("Category name must not exceed 100 characters.");
    }
}

