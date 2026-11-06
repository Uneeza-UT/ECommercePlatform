using ECommercePlatform.Application.DTOs.Common;
using ECommercePlatform.Application.DTOs.Coupon;


namespace ECommercePlatform.Application.Contracts.Services
{
    public interface ICouponService
    {
        Task<List<GetCouponDto>> GetAsync(PaginationDto dto);
        Task<GetCouponDto> GetByIdAsync(int id);
        Task<int> CreateAsync(CreateCouponDto dto);
        Task UpdateAsync(int id, UpdateCouponDto dto);
        Task ChangeStatusAsync(int id, ChangeCouponStatusDto dto);
        Task AssignToCustomerAsync(int id, AssignCouponDto dto);
        Task DeleteAsync(int id);
    }
}
