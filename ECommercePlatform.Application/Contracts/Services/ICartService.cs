using ECommercePlatform.Application.DTOs.Cart;

namespace ECommercePlatform.Application.Contracts.Services
{
    public interface ICartService
    {
        Task<GetCartDto> GetAsync();
        Task CreateAsync(); 
    }
}
