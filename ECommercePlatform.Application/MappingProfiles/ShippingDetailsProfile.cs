using AutoMapper;
using ECommercePlatform.Application.DTOs.ShippingDetails;
using ECommercePlatform.Domain;

namespace ECommercePlatform.Application.MappingProfiles
{
    public class ShippingDetailsProfile : Profile
    {
        public ShippingDetailsProfile()
        {
            CreateMap<ShippingDetails, GetShippingDetailsDto>();
            CreateMap<CreateShippingDetailsDto, ShippingDetails>();
        }
    }
}
