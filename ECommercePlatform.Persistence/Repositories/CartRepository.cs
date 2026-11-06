using ECommercePlatform.Application.Contracts.Persistence;
using ECommercePlatform.Domain;
using ECommercePlatform.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace ECommercePlatform.Persistence.Repositories
{
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        public CartRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            
        }


        public async Task<Cart?> GetByUserIdAsync(string userId)
        {
            return await _dbContext.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }


        public async Task ClearCartAsync(string userId)
        {
            var cart = await _dbContext.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                return;

            _dbContext.CartItems.RemoveRange(cart.CartItems);
            await _dbContext.SaveChangesAsync();
        }
    }
}
