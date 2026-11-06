using ECommercePlatform.Domain;

namespace ECommercePlatform.Application.Contracts.Persistence
{
    public interface ICouponRepository : IGenericRepository<Coupon>
    {
        Task<IReadOnlyList<Coupon>> GetAllWithDetailsAsync(int pageNumber = 1, int pageSize = 10);
        Task<Coupon?> GetByIdWithDetailsAsync(int id);
        Task<Coupon?> GetByCodeAsync(string code);
        Task<bool> IsCodeUnique(string code, int? excludeId = null);
        Task ExpireCouponsAsync();
    }
}
