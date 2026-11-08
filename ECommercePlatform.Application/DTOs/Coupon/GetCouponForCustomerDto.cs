using ECommercePlatform.Domain.Enums;

namespace ECommercePlatform.Application.DTOs.Coupon
{
    public class GetCouponForCustomerDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public CouponStatus CouponStatus { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal MinimumOrderAmount { get; set; }
        public decimal? MaximumDiscount { get; set; }
    }
}
