using ECommercePlatform.Application.DTOs.CartItem;
using FluentValidation;

namespace ECommercePlatform.Application.Validations.CartItem
{
    public class AddCartItemValidator : AbstractValidator<AddCartItemDto>
    {
        public AddCartItemValidator()
        {
            RuleFor(p => p.ProductId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than zero.");


            RuleFor(p => p.Quantity)
               .NotEmpty().WithMessage("{PropertyName} is required.")
               .GreaterThan(0).WithMessage("{PropertyName} must be greater than zero.");

        }
    }
}
