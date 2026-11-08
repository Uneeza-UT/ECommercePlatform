using AutoMapper;
using ECommercePlatform.Application.DTOs.Image;
using ECommercePlatform.Domain;

namespace ECommercePlatform.Application.MappingProfiles
{
    public class ImageProfile : Profile
    {
        public ImageProfile()
        {
            CreateMap<Image, GetImageDto>();
        }
    }
}
