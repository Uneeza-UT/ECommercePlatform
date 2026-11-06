namespace ECommercePlatform.Application.Contracts.Persistence
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IReadOnlyList<T>> GetAsync(int pageNumber = 1, int pageSize = 10);
        Task<T?> GetByIdAsync(int id);
        Task<T> CreateAsync(T entity);
        Task SaveChangesAsync();
    }
}
