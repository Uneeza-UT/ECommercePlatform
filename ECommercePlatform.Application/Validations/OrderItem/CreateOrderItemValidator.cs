using ECommercePlatform.Application.DTOs.OrderItem;
using FluentValidation;

namespace ECommercePlatform.Application.Validations.OrderItem
{
    public class CreateOrderItemValidator : AbstractValidator<CreateOrderItemDto>
    {
        public CreateOrderItemValidator()
        {
            RuleFor(oi => oi.ProductId)
                .GreaterThan(0).WithMessage("ProductId must be greater than 0.");

            RuleFor(oi => oi.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.");
        }
    }
}
