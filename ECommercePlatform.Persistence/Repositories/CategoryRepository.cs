using ECommercePlatform.Application.Contracts.Persistence;
using ECommercePlatform.Domain;
using ECommercePlatform.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace ECommercePlatform.Persistence.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }


        public async Task<IReadOnlyList<Category>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
        {
            return await _dbContext.Set<Category>()
                .AsNoTracking()
                .Where(c => !c.IsDeleted)
                .OrderByDescending(c => c.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }


        public async Task<Category?> GetByIdWithProductsAsync(int id)
        {
            return await _dbContext.Set<Category>()
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
        }

        public async Task<bool> IsNameUnique(string name, int? excludeId = null)
        {
            return !await _dbContext.Categories
                .AnyAsync(c => c.Name == name &&
                    !c.IsDeleted &&
                    (!excludeId.HasValue || c.Id != excludeId.Value));
        }
    }
}
