using ECommercePlatform.Domain;

namespace ECommercePlatform.Application.Contracts.Persistence
{
    public interface ICartRepository : IGenericRepository<Cart>
    {
        Task<Cart?> GetByUserIdAsync(string userId);
        Task ClearCartAsync(string userId);
    }
}
