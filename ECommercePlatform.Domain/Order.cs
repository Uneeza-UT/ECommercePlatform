using ECommercePlatform.Domain.Common;
using ECommercePlatform.Domain.Enums;

namespace ECommercePlatform.Domain
{
    public class Order : BaseEntity
    {
        public string? UserId { get; set; }
        public decimal SubTotal { get; set; }      
        public decimal? DiscountAmount { get; set; }
        public decimal DeliveryFee { get; set; }
        public decimal TotalAmount { get; set; }
        public PaymentType PaymentType { get; set; }
        public OrderStatus OrderStatus { get; set; }

        public ShippingDetails ShippingDetails { get; set; } = null!;
        public CouponUsage? CouponUsage { get; set; }  
        public Payment Payment { get; set; } = null!;

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();    
    }
}
