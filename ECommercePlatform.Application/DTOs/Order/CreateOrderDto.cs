using ECommercePlatform.Application.DTOs.OrderItem;
using ECommercePlatform.Application.DTOs.ShippingDetails;
using ECommercePlatform.Domain.Enums;

namespace ECommercePlatform.Application.DTOs.Order
{
    public class CreateOrderDto
    {
        public CreateShippingDetailsDto ShippingDetails { get; set; }
        public string? CouponCode { get; set; }
        public PaymentType PaymentType { get; set; }    
        public List<CreateOrderItemDto>? OrderItems { get; set; } = new List<CreateOrderItemDto>();
    }
}
