using AutoMapper;
using ECommercePlatform.Application.DTOs.Order;
using ECommercePlatform.Domain;

namespace ECommercePlatform.Application.MappingProfiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, GetOrderDto>();
            CreateMap<CreateOrderDto, Order>();
        }
    }
}
