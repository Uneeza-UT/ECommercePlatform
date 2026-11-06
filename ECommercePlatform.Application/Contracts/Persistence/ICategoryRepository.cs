using ECommercePlatform.Domain;

namespace ECommercePlatform.Application.Contracts.Persistence
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<IReadOnlyList<Category>> GetAllAsync(int pageNumber = 1, int pageSize = 10);
        Task<Category?> GetByIdWithProductsAsync(int id);
        Task<bool> IsNameUnique(string name, int? excludeId = null);
    }
}
