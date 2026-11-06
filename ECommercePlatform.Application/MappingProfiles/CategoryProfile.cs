using AutoMapper;
using ECommercePlatform.Application.DTOs.Category;
using ECommercePlatform.Domain;

namespace ECommercePlatform.Application.MappingProfiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, GetCategoryDto>();
            CreateMap<CreateCategoryDto, Category>();
        }
    }
}
