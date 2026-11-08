using AutoMapper;
using ECommercePlatform.Application.DTOs.Coupon;
using ECommercePlatform.Domain;

namespace ECommercePlatform.Application.MappingProfiles
{
    public class CouponProfile : Profile
    {
        public CouponProfile()
        {
            CreateMap<Coupon, GetCouponDto>();
            CreateMap<Coupon, GetCouponForCustomerDto>();
            CreateMap<CreateCouponDto, Coupon>();
            CreateMap<UpdateCouponDto, Coupon>();
        }
    }
}
