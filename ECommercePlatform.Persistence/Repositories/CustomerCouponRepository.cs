using ECommercePlatform.Application.Contracts.Persistence;
using ECommercePlatform.Domain;
using ECommercePlatform.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace ECommercePlatform.Persistence.Repositories
{
    public class CustomerCouponRepository : GenericRepository<CustomerCoupon>, ICustomerCouponRepository
    {
        public CustomerCouponRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }


        public async Task<IReadOnlyList<CustomerCoupon>> GetAllByCustomerIdAsync(
            string customerId, int pageNumber = 1, int pageSize = 10)
        {
            return await _dbContext.Set<CustomerCoupon>()
                .AsNoTracking()
                .Where(cc => cc.CustomerId == customerId)
                .OrderByDescending(cc => cc.AssignedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        
    }
}
