using ECommercePlatform.Application.DTOs.CouponUsage;
using ECommercePlatform.Domain.Enums;

namespace ECommercePlatform.Application.DTOs.Coupon
{
    public class GetCouponDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public CouponType CouponType { get; set; }
        public CouponStatus CouponStatus { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal MinimumOrderAmount { get; set; }
        public decimal? MaximumDiscount { get; set; }
        public int? AssignmentLimit { get; set; }
        public int AssignedCount { get; set; }
        public int UsedCount { get; set; }
        public int? ValidityDays { get; set; }
        public DateTime? StartsAt { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<GetCouponUsagesDto> CouponUsages { get; set; } = new List<GetCouponUsagesDto>();
    }
}
