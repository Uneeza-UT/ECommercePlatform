using AutoMapper;
using ECommercePlatform.Application.DTOs.Cart;
using ECommercePlatform.Domain;

namespace ECommercePlatform.Application.MappingProfiles
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<Cart, GetCartDto>();
        }
    }
}
