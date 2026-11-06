using AutoMapper;
using ECommercePlatform.Application.Contracts.Identity;
using ECommercePlatform.Application.Contracts.Persistence;
using ECommercePlatform.Application.Contracts.Services;
using ECommercePlatform.Application.DTOs.Common;
using ECommercePlatform.Application.DTOs.CustomerCoupon;

namespace ECommercePlatform.Infrastructure.Services
{
    public class CustomerCouponService : ICustomerCouponService
    {
        private IMapper _mapper;
        private readonly ICustomerCouponRepository _customerCouponRepository;
        private readonly ICurrentUserService _currentUserService;

        public CustomerCouponService(IMapper mapper, 
            ICustomerCouponRepository customerCouponRepository, 
            ICurrentUserService currentUserService)
        {
            _mapper = mapper;
            _customerCouponRepository = customerCouponRepository;
            _currentUserService = currentUserService;
        }

        public async Task<List<GetCustomerCouponDto>> GetAsync(PaginationDto dto)
        {
            string userId = _currentUserService.UserId;
            var customerCoupons = await _customerCouponRepository
                .GetAllByCustomerIdAsync(userId, dto.PageNumber, dto.PageSize);

            return _mapper.Map<List<GetCustomerCouponDto>>(customerCoupons);
        }
    }
}
