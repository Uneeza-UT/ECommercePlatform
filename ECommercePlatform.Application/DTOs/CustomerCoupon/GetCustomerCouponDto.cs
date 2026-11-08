using ECommercePlatform.Application.DTOs.Coupon;

namespace ECommercePlatform.Application.DTOs.CustomerCoupon
{
    public class GetCustomerCouponDto
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public string GuestEmail { get; set; }
        public bool IsUsed { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime? AssignedAt { get; set; }
        public GetCouponForCustomerDto CustomerCoupon { get; set; }
    }
}
