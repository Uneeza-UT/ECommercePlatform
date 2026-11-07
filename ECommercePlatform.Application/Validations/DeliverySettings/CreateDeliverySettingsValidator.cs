using ECommercePlatform.Application.DTOs.DeliverySettings;
using FluentValidation;

namespace ECommercePlatform.Application.Validations.DeliverySettings
{
    public class CreateDeliverySettingsValidator : AbstractValidator<CreateDeliverySettingsDto>
    {
        public CreateDeliverySettingsValidator()
        {
            RuleFor(ds => ds.CashOnDeliveryFee)
               .GreaterThan(0).WithMessage("{PropertyName} must be greater than zero.");


            RuleFor(ds => ds.OnlinePaymentDeliveryFee)
               .GreaterThan(0).WithMessage("{PropertyName} must be greater than zero.");
        }
    }
}
