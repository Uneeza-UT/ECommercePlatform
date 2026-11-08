using ECommercePlatform.Application.Contracts.Persistence;
using ECommercePlatform.Application.Enums;
using ECommercePlatform.Domain;
using ECommercePlatform.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ECommercePlatform.Persistence.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }


        public async Task<IReadOnlyList<Product>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
        {
            return await _dbContext.Set<Product>()
                .AsNoTracking()
                .Where(p => !p.IsDeleted)
                .OrderByDescending(p => p.CreatedAt)
                .Include(p => p.Images)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }



        public async Task<IReadOnlyList<Product>> GetAllByCategoryIdAsync
            (int categoryId, int pageNumber = 1, int pageSize = 10)
        {
            return await _dbContext.Set<Product>()
                .AsNoTracking()
                .Where(p => p.CategoryId == categoryId && !p.IsDeleted)
                .OrderByDescending(p => p.CreatedAt)
                .Include(p => p.Images)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }



        public async Task<Product?> GetByIdWithImagesAsync(int id)
        {
            return await _dbContext.Set<Product>()
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
        }




        public async Task<IReadOnlyList<Product>> GetAllByIdsAsync(List<int> ids)
        {
            return await _dbContext.Products
                .Where(p => ids.Contains(p.Id) && !p.IsDeleted)
                .Include(p => p.Images)
                .ToListAsync();
        }



        public async Task<bool> IsNameUnique(string name, int? excludeId = null)
        {
            return !await _dbContext.Products
                .AnyAsync(p => p.Name == name &&
                !p.IsDeleted &&
                (!excludeId.HasValue || p.Id != excludeId.Value));
        }


        public async Task<IReadOnlyList<Product>> SearchAsync(string searchTerm, int pageNumber = 1, int pageSize = 10)
        {
            return await _dbContext.Set<Product>()
                .AsNoTracking()
                .Where(p =>!p.IsDeleted &&
                    (p.Name.Contains(searchTerm) ||
                    p.Description.Contains(searchTerm)))
                .Include(p => p.Images)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }


        public async Task<IReadOnlyList<Product>> SortAsync<TKey>(
            bool descending, Expression<Func<Product, TKey>> keySelector, int pageNumber = 1, int pageSize = 10)
        {
            var query = _dbContext.Set<Product>()
                .AsNoTracking()
                .Where(p => !p.IsDeleted);

            query = descending ? query.OrderByDescending(keySelector) : query.OrderBy(keySelector);

            return await query
                .Include(p => p.Images)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }


        public async Task<IReadOnlyList<Product>> FilterAsync(FilterOption option, int pageNumber = 1, int pageSize = 10)
        {
            var query = _dbContext.Set<Product>().AsNoTracking();

            if (option == FilterOption.InStock)
            {
                query = query.Where(p => p.StockQuantity > 0 && !p.IsDeleted);
            }

            else if (option == FilterOption.OutOfStock)
            {
                query = query.Where(p => p.StockQuantity == 0 && !p.IsDeleted);
            }

            return await query
                .Include(p => p.Images)
                .OrderByDescending(p => p.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
