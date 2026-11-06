using ECommercePlatform.Application.DTOs.Coupon;
using ECommercePlatform.Domain.Enums;
using FluentValidation;

namespace ECommercePlatform.Application.Validations.Coupon
{
    public class ChangeCouponStatusValidator : AbstractValidator<ChangeCouponStatusDto>
    {
        public ChangeCouponStatusValidator()
        {
            RuleFor(c => c.CouponStatus)
                .Must(status =>
                    status == CouponStatus.Active ||
                    status == CouponStatus.InActive)
                .WithMessage("Allowed values: Active, InActive.");
        }
    }
}
