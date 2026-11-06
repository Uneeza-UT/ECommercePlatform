using ECommercePlatform.Domain.Common;
using ECommercePlatform.Domain.Enums;

namespace ECommercePlatform.Domain
{
    public class Coupon : BaseEntity
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
        public int AssignedCount { get; set; }
        public int UsedCount { get; set; }
        public int? ValidityDays { get; set; }
        public DateTime? StartsAt { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool IsDeleted { get; set; }       
        public DateTime? UpdatedAt { get; set; }

        public ICollection<CouponUsage> CouponUsages { get; set; } = new List<CouponUsage>();
        public ICollection<CustomerCoupon> CustomerCoupons { get; set; } = new List<CustomerCoupon>();
    }
}
