using AutoMapper;
using ECommercePlatform.Application.DTOs.CouponUsage;
using ECommercePlatform.Domain;

namespace ECommercePlatform.Application.MappingProfiles
{
    public class CouponUsageProfile : Profile
    {
        public CouponUsageProfile()
        {
            CreateMap<CouponUsage, GetCouponUsagesDto>();
        }
    }
}
