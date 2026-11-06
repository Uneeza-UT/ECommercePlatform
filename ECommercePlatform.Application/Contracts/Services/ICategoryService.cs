using ECommercePlatform.Application.DTOs.Category;
using ECommercePlatform.Application.DTOs.Common;

namespace ECommercePlatform.Application.Contracts.Services
{
    public interface ICategoryService
    {
        Task<List<GetCategoryDto>> GetAsync(PaginationDto dto);
        Task<GetCategoryDto> GetByIdAsync(int id);
        Task<int> CreateAsync(CreateCategoryDto dto);
        Task UpdateAsync(int id, UpdateCategoryDto dto);
        Task DeleteAsync(int id);
    }
}
