using ECommercePlatform.Application.DTOs.Common;
using ECommercePlatform.Application.DTOs.Product;

namespace ECommercePlatform.Application.Contracts.Services
{
    public interface IProductService
    {
        Task<List<GetProductsDto>> GetAsync(int categoryId, PaginationDto dto);
        Task<GetProductsDto> GetByIdAsync(int id);
        Task<int> CreateAsync(CreateProductDto dto);
        Task UpdateAsync(int id, UpdateProductDto dto);
        Task DeleteAsync(int id);   
        Task<List<GetProductsDto>> SearchAsync(SearchDto dto, PaginationDto paginationDto);
        Task<List<GetProductsDto>> SortAsync(SortDto dto, PaginationDto paginationDto);
        Task<List<GetProductsDto>> FilterAsync(FilterDto dto, PaginationDto paginationDto);
    }
}
