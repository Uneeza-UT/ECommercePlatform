using ECommercePlatform.Application.DTOs.Coupon;
using FluentValidation;

namespace ECommercePlatform.Application.Validations.Coupon
{
    public class AssignCouponValidator : AbstractValidator<AssignCouponDto>
    {
        public AssignCouponValidator()
        {
            RuleFor(c => c)
                .Must(c =>
                    (c.CustomerId != null && c.GuestEmail == null) ||
                    (c.CustomerId == null && !string.IsNullOrWhiteSpace(c.GuestEmail)))
                .WithMessage("Either CustomerId or GuestEmail must be provided, but not both.");


            RuleFor(c => c.GuestEmail)              
                .Matches(@"^[^@\s]+@[^@\s.]+(\.[^@\s.]+)+$").WithMessage("{PropertyName} must be a valid email address.")
                .MaximumLength(500).WithMessage("{PropertyName} cannot exceed 500 characters.")
                .When(c => !string.IsNullOrEmpty(c.GuestEmail));
        }
    }
}
