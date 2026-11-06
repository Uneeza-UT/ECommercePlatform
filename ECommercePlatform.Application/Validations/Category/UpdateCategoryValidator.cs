using ECommercePlatform.Application.DTOs.Category;
using FluentValidation;

namespace ECommercePlatform.Application.Validations.Category
{
    public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryDto>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(100).WithMessage("{PropertyName} cannot exceed 100 characters.");
        }
    }
}
