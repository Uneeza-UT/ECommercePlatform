using AutoMapper;
using ECommercePlatform.Application.DTOs.DeliverySettings;
using ECommercePlatform.Domain;

namespace ECommercePlatform.Application.MappingProfiles
{
    public class DeliverySettingsProfile : Profile
    {
        public DeliverySettingsProfile()
        {
            CreateMap<DeliverySettings, GetDeliverySettingsDto>();
            CreateMap<CreateDeliverySettingsDto, DeliverySettings>();
            CreateMap<UpdateDeliverySettingsDto, DeliverySettings>();
        }
    }
}
