using ECommercePlatform.Domain.Common;

namespace ECommercePlatform.Domain
{
    public class CouponUsage : BaseEntity
    {
        public int CouponId { get; set; }
        public Coupon? Coupon { get; set; }

        public int OrderId { get; set; }
        public Order? Order { get; set; }
    }
}
