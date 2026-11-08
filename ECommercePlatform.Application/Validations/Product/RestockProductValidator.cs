using ECommercePlatform.Application.DTOs.Product;
using FluentValidation;

namespace ECommercePlatform.Application.Validations.Product
{
    public class RestockProductValidator : AbstractValidator<RestockProductDto>
    {
        public RestockProductValidator()
        {
            RuleFor(p => p.StockQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("{PropertyName} cannot be negative.");
        }
    }
}
