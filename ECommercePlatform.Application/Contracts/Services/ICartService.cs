using ECommercePlatform.Application.DTOs.Cart;
using ECommercePlatform.Application.DTOs.CartItem;

namespace ECommercePlatform.Application.Contracts.Services
{
    public interface ICartService
    {
        Task<GetCartDto> GetAsync();
        Task CreateAsync();
        Task AddItemToCartAsync(AddCartItemDto dto);
    }
}
