using ECommercePlatform.Application.DTOs.DeliverySettings;
using FluentValidation;

namespace ECommercePlatform.Application.Validations.DeliverySettings
{
    public class UpdateDeliverySettingsValidator : AbstractValidator<UpdateDeliverySettingsDto>
    {
        public UpdateDeliverySettingsValidator()
        {
            RuleFor(ds => ds.CashOnDeliveryFee)
               .NotEmpty().WithMessage("{PropertyName} is required.")
               .GreaterThan(0).WithMessage("{PropertyName} must be greater than zero.");


            RuleFor(ds => ds.OnlinePaymentDeliveryFee)
               .NotEmpty().WithMessage("{PropertyName} is required.")
               .GreaterThan(0).WithMessage("{PropertyName} must be greater than zero.");
        }
    }
}
