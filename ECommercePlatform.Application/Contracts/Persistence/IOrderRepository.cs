using ECommercePlatform.Domain;

namespace ECommercePlatform.Application.Contracts.Persistence
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<IReadOnlyList<Order>> GetAllWithDetailsAsync(int pageNumber = 1, int pageSize = 10);
        Task<IReadOnlyList<Order>> GetAllByUserIdWithDetailsAsync(
            string userId, int pageNumber = 1, int pageSize = 10);
        Task<Order?> GetByIdWithDetailsAsync(int id);
        Task<Order?> GetFirstOrderAsync(string? userId, string? guestEmail);
    }
}
