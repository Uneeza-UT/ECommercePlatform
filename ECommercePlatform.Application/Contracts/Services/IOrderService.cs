using ECommercePlatform.Application.DTOs.Common;
using ECommercePlatform.Application.DTOs.Order;

namespace ECommercePlatform.Application.Contracts.Services
{
    public interface IOrderService
    {
        Task<List<GetOrderDto>> GetAsync(PaginationDto dto);
        Task<GetOrderDto> GetByIdAsync(int id);
        Task<int> CreateAsync(CreateOrderDto dto);
        Task ChangeOrderStatusAsync(int id, ChangeOrderStatusDto dto);
    }
}
