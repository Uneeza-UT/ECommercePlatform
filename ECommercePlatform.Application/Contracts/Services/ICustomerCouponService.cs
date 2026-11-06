using ECommercePlatform.Application.DTOs.Common;
using ECommercePlatform.Application.DTOs.CustomerCoupon;

namespace ECommercePlatform.Application.Contracts.Services
{
    public interface ICustomerCouponService
    {
        Task<List<GetCustomerCouponDto>> GetAsync(PaginationDto dto);  
    }
}
