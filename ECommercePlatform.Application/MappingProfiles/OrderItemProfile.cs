using AutoMapper;
using ECommercePlatform.Application.DTOs.OrderItem;
using ECommercePlatform.Domain;

namespace ECommercePlatform.Application.MappingProfiles
{
    public class OrderItemProfile : Profile
    {
        public OrderItemProfile()
        {
            CreateMap<OrderItem, GetOrderItemDto>();
            CreateMap<CreateOrderItemDto, OrderItem>();
        }
    }
}
