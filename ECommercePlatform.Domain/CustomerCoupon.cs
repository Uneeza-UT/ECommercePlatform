using ECommercePlatform.Domain.Common;

namespace ECommercePlatform.Domain
{
    public class CustomerCoupon : BaseEntity
    {
        public int CouponId { get; set; }
        public Coupon Coupon { get; set; }
        public string? CustomerId { get; set; }
        public string? GuestEmail { get; set; }
        public bool IsUsed { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime? AssignedAt { get; set; }
        public byte[] RowVersion { get; set; } = null!;
    }
}
