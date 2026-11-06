using ECommercePlatform.Application.DTOs.Coupon;
using ECommercePlatform.Domain.Enums;
using FluentValidation;

namespace ECommercePlatform.Application.Validations.Coupon
{
    public class UpdateCouponValidator : AbstractValidator<UpdateCouponDto>
    {
        public UpdateCouponValidator()
        {
            RuleFor(c => c.Code)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(100).WithMessage("{PropertyName} cannot exceed 100 characters.");


            RuleFor(c => c.CouponType)
                .IsInEnum().WithMessage("Allowed values: Percentage, FixedAmount.");


            RuleFor(c => c.CouponStatus)
                .Must(status =>
                    status == CouponStatus.Active ||
                    status == CouponStatus.InActive)
                .WithMessage("Allowed values: Active, InActive.");


            RuleFor(c => c.AssignmentType)
                .IsInEnum().WithMessage("Allowed values: Public, CustomerSpecific.");


            RuleFor(c => c.DiscountPercentage)
                .NotNull()
                .InclusiveBetween(0.01m, 100m)
                .When(c => c.CouponType == CouponType.Percentage)
                .WithMessage("{PropertyName} must be between 0.01 and 100.");


            RuleFor(c => c.DiscountPercentage)
                .Null()
                .When(c => c.CouponType == CouponType.FixedAmount)
                .WithMessage("{PropertyName} must be null for a fixed-amount coupon.");


            RuleFor(c => c.DiscountAmount)
                .NotNull()
                .GreaterThan(0)
                .When(c => c.CouponType == CouponType.FixedAmount)
                .WithMessage("{PropertyName} must be greater than zero for a fixed-amount coupon.");


            RuleFor(c => c.DiscountAmount)
                .Null()
                .When(c => c.CouponType == CouponType.Percentage)
                .WithMessage("{PropertyName} must be null for a percentage coupon.");


            RuleFor(c => c.MinimumOrderAmount)
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than zero.");


            RuleFor(c => c.MaximumDiscount)
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than zero.");


            RuleFor(c => c.AssignmentLimit)
                .GreaterThan(0)
                .When(c => c.AssignmentLimit.HasValue)
                .WithMessage("{PropertyName} must be greater than zero.");


            RuleFor(c => c.ValidityDays)
                .GreaterThan(0)
                .When(c => c.ValidityDays.HasValue)
                .WithMessage("{PropertyName} must be greater than zero.");


            RuleFor(c => c.StartsAt)
               .GreaterThan(DateTime.Now)
               .When(c => c.StartsAt.HasValue)
               .WithMessage("{PropertyName} must be in the future.");


            RuleFor(c => c.StartsAt)
                .LessThan(c => c.ExpiryDate)
                .When(c => c.StartsAt.HasValue && c.ExpiryDate.HasValue)
                .WithMessage("{PropertyName} must be before expiry date.");


            RuleFor(c => c.ExpiryDate)
                .GreaterThan(DateTime.Now)
                .When(c => c.ExpiryDate.HasValue)
                .WithMessage("{PropertyName} must be in the future.");


            RuleFor(c => c.ExpiryDate)
                .GreaterThan(c => c.StartsAt)
                .When(c => c.StartsAt.HasValue && c.ExpiryDate.HasValue)
                .WithMessage("{PropertyName} must be after start date.");
        }
    }
}
