using ECommercePlatform.Application.Contracts.Identity;
using ECommercePlatform.Application.DTOs.Order;
using ECommercePlatform.Application.Validations.OrderItem;
using ECommercePlatform.Application.Validations.ShippingDetails;
using FluentValidation;

namespace ECommercePlatform.Application.Validations.Order
{
    public class CreateOrderValidator : AbstractValidator<CreateOrderDto>
    {
        private readonly ICurrentUserService _currentUserService;
        public CreateOrderValidator(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;


            RuleFor(o => o.ShippingDetails)
                .NotNull()
                .SetValidator(new CreateShippingDetailsValidator());


            RuleFor(o => o.OrderItems)
                .NotEmpty()
                .When(o => !_currentUserService.IsAuthenticated)
                .WithMessage("{PropertyName} are required for unauthenticated users.");


            RuleFor(o => o.OrderItems)
                .Must(items => items.Select(p => p.ProductId).Distinct().Count() == items.Count)
                .WithMessage("Duplicate products are not allowed in order items.")
                .When(o => o.OrderItems != null && o.OrderItems.Any());


            RuleForEach(o => o.OrderItems)
                .SetValidator(new CreateOrderItemValidator())
                .When(o => !_currentUserService.IsAuthenticated);


            RuleFor(o => o.PaymentType)
                .IsInEnum().WithMessage("Allowed values: CashOnDelivery, CreditDebitCard, BankTransfer.");


            RuleFor(o => o.CouponCode)
                .MaximumLength(100).WithMessage("{PropertyName} cannot exceed 100 characters.");
        }
    }
}
