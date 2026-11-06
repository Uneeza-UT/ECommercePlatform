using AutoMapper;
using ECommercePlatform.Application.DTOs.CustomerCoupon;
using ECommercePlatform.Domain;

namespace ECommercePlatform.Application.MappingProfiles
{
    public class CustomerCouponProfile : Profile
    {
        public CustomerCouponProfile()
        {
            CreateMap<CustomerCoupon, GetCustomerCouponDto>();
        }
    }
}
