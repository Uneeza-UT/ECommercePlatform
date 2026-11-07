using ECommercePlatform.Application.DTOs.CartItem;

namespace ECommercePlatform.Application.Contracts.Services
{
    public interface ICartItemService
    {
        Task<List<GetCartItemDto>> AddItemToCartAsync(int productId, AddCartItemDto dto);
        Task UpdateQuantityAsync(int productId, bool increment);
        Task RemoveItemFromCartAsync(int productId);
    }
}
