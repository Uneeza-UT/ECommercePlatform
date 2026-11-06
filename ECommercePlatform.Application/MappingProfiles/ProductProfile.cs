using AutoMapper;
using ECommercePlatform.Application.DTOs.Product;
using ECommercePlatform.Domain;

namespace ECommercePlatform.Application.MappingProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, GetProductsDto>();
            CreateMap<CreateProductDto, Product>();
            CreateMap<UpdateProductDto, Product>();
        }
    }
}
