using ECommercePlatform.Application.DTOs.CouponUsage;
using ECommercePlatform.Application.DTOs.OrderItem;
using ECommercePlatform.Application.DTOs.Payment;
using ECommercePlatform.Domain.Enums;

namespace ECommercePlatform.Application.DTOs.Order
{
    public class GetOrderDto
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public int ShippingDetailsId { get; set; }

        public GetCouponUsagesDto? CouponUsage { get; set; }

        public decimal SubTotal { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public PaymentType PaymentType { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public GetPaymentDto? Payment { get; set; }
        public List<GetOrderItemDto> OrderItems { get; set; } = new List<GetOrderItemDto>();   
    }
}
