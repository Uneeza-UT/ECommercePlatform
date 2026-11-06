namespace ECommercePlatform.Application.Contracts.Services
{
    public interface ICartItemService
    {
        Task UpdateQuantityAsync(int productId, bool increment);
        Task RemoveItemFromCartAsync(int productId);
    }
}
