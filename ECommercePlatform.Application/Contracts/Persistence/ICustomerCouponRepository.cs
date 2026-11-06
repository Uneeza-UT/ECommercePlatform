using ECommercePlatform.Domain;

namespace ECommercePlatform.Application.Contracts.Persistence
{
    public interface ICustomerCouponRepository : IGenericRepository<CustomerCoupon>
    {
        Task<IReadOnlyList<CustomerCoupon>> GetAllByCustomerIdAsync(
            string customerId, int pageNumber = 1, int pageSize = 10);   
    }
}
