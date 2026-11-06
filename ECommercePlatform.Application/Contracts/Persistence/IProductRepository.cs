using ECommercePlatform.Application.Enums;
using ECommercePlatform.Domain;
using System.Linq.Expressions;

namespace ECommercePlatform.Application.Contracts.Persistence
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<IReadOnlyList<Product>> GetAllAsync(int pageNumber = 1, int pageSize = 10);
        Task<IReadOnlyList<Product>> GetAllByCategoryIdAsync(int categoryId, int pageNumber = 1, int pageSize = 10);
        Task<IReadOnlyList<Product>> GetAllByIdsAsync(List<int> ids);
        Task<bool> IsNameUnique(string name, int? excludeId = null);
        Task<IReadOnlyList<Product>> SearchAsync(string searchTerm, int pageNumber = 1, int pageSize = 10);
        Task<IReadOnlyList<Product>> SortAsync<TKey>(
            bool descending, Expression<Func<Product, TKey>> keySelector, int pageNumber = 1, int pageSize = 10);
        Task<IReadOnlyList<Product>> FilterAsync(FilterOption option, int pageNumber = 1, int pageSize = 10);
    }
}
