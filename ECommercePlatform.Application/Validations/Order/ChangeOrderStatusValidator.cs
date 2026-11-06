using ECommercePlatform.Application.DTOs.Order;
using ECommercePlatform.Domain.Enums;
using FluentValidation;

namespace ECommercePlatform.Application.Validations.Order
{
    public class ChangeOrderStatusValidator : AbstractValidator<ChangeOrderStatusDto>
    {
        public ChangeOrderStatusValidator()
        {
            RuleFor(o => o.OrderStatus)
                .Must(status =>
                    status == OrderStatus.Confirmed ||
                    status == OrderStatus.Shipped ||
                    status == OrderStatus.Delivered ||
                    status == OrderStatus.Cancelled)
                .WithMessage("Allowed values: Confirmed, Shipped, Delivered, Cancelled.");
        }
    }
}
