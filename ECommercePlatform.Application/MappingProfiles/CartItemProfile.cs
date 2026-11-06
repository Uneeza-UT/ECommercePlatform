using AutoMapper;
using ECommercePlatform.Application.DTOs.CartItem;
using ECommercePlatform.Domain;

namespace ECommercePlatform.Application.MappingProfiles
{
    public class CartItemProfile : Profile
    {
        public CartItemProfile()
        {
            CreateMap<CartItem, GetCartItemDto>();
        }
    }
}
