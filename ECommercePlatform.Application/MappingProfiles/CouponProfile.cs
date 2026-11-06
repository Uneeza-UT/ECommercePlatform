using AutoMapper;
using ECommercePlatform.Application.DTOs.Coupon;
using ECommercePlatform.Domain;

namespace ECommercePlatform.Application.MappingProfiles
{
    public class CouponProfile : Profile
    {
        public CouponProfile()
        {
            CreateMap<Order, GetCouponDto>();
            CreateMap<CreateCouponDto, Order>();
            CreateMap<UpdateCouponDto, Order>();
        }
    }
}
