using ECommercePlatform.Application.Contracts.Persistence;
using ECommercePlatform.Domain;
using ECommercePlatform.Domain.Enums;
using ECommercePlatform.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace ECommercePlatform.Persistence.Repositories
{
    public class CouponRepository : GenericRepository<Coupon>, ICouponRepository
    {
        public CouponRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            
        }

        public async Task<IReadOnlyList<Coupon>> GetAllWithDetailsAsync(int pageNumber = 1, int pageSize = 10)
        {
            return await _dbContext.Set<Coupon>()
                .AsNoTracking()
                .Where(c => !c.IsDeleted)
                .OrderByDescending(c => c.CreatedAt)
                .Include(c => c.CouponUsages)
                .Include(c => c.CustomerCoupons)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }


        public async Task<Coupon?> GetByIdWithDetailsAsync(int id)
        {
            return await _dbContext.Set<Coupon>()
                .Include(c => c.CouponUsages)
                .Include(c => c.CustomerCoupons)
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
        }



        public async Task<Coupon?> GetByCodeAsync(string code)
        {
            return await _dbContext.Coupons
                .Include(c => c.CouponUsages)
                .Include(c => c.CustomerCoupons)
                .FirstOrDefaultAsync(c => c.Code == code && !c.IsDeleted);
        }



        public async Task<bool> IsCodeUnique(string code, int? excludeId = null)
        {       
            return !await _dbContext.Coupons
                .AnyAsync(c => c.Code == code && 
                    !c.IsDeleted && 
                    (!excludeId.HasValue || c.Id != excludeId.Value));
        }



        //Changes the status of coupons whose expiry date has passed to Expired
        public async Task ExpireCouponsAsync()
        {
            var coupons = await _dbContext.Coupons
                .Where(c =>
                    c.ExpiryDate.HasValue &&
                    c.ExpiryDate <= DateTime.Now &&
                    c.CouponStatus != CouponStatus.Expired)
                .ToListAsync();

            foreach (var coupon in coupons)
            {
                coupon.CouponStatus = CouponStatus.Expired;
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
