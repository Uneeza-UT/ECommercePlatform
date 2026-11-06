using ECommercePlatform.Application.Contracts.Persistence;
using ECommercePlatform.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace ECommercePlatform.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _dbContext;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<IReadOnlyList<T>> GetAsync(int pageNumber = 1, int pageSize = 10)
        {
            return await _dbContext.Set<T>()
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }



        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }



        public async Task<T> CreateAsync(T entity)
        {
            await _dbContext.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }



        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
