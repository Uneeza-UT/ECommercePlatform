using ECommercePlatform.Application.Contracts.Persistence;
using ECommercePlatform.Domain;
using ECommercePlatform.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace ECommercePlatform.Persistence.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<IReadOnlyList<Order>> GetAllWithDetailsAsync(int pageNumber = 1, int pageSize = 10)
        {
            return await _dbContext.Set<Order>()
                .AsNoTracking()
                .OrderByDescending(o => o.CreatedAt)
                .Include(o => o.OrderItems)
                .Include(o => o.Payment)
                .Include(o => o.ShippingDetails)
                .Include(o => o.CouponUsage)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }



        public async Task<IReadOnlyList<Order>> GetAllByUserIdWithDetailsAsync(
            string userId, int pageNumber = 1, int pageSize = 10)
        {
            return await _dbContext.Set<Order>()
                .AsNoTracking()
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .Include(o => o.OrderItems)
                .Include(o => o.Payment)
                .Include(o => o.ShippingDetails)
                .Include(o => o.CouponUsage)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }


        public async Task<Order?> GetByIdWithDetailsAsync(int id)
        {
            return await _dbContext.Set<Order>()
                .Include(o => o.OrderItems)
                .Include(o => o.Payment)
                .Include(o => o.ShippingDetails)
                .Include(o => o.CouponUsage)
                .FirstOrDefaultAsync(o => o.Id == id);
        }



        public async Task<Order?> GetFirstOrderAsync(string? userId, string? guestEmail)
        {
            return await _dbContext.Orders
                .FirstOrDefaultAsync(o =>
                    (userId != null && o.UserId == userId) ||
                    (guestEmail != null &&
                     o.UserId == null &&
                     o.ShippingDetails.Email == guestEmail));
        }
    }
}
