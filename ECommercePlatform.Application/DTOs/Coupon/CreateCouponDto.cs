using ECommercePlatform.Domain.Enums;

namespace ECommercePlatform.Application.DTOs.Coupon
{
    public class CreateCouponDto
    {
        public string Code { get; set; }
        public CouponType CouponType { get; set; }
        public CouponStatus CouponStatus { get; set; }
        public CouponAssignmentType AssignmentType { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal MinimumOrderAmount { get; set; }
        public decimal? MaximumDiscount { get; set; }
        public int? AssignmentLimit { get; set; }
        public int? ValidityDays { get; set; }
        public DateTime? StartsAt { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }
}
