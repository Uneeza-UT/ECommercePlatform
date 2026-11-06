using ECommercePlatform.Application.Contracts.Services;
using ECommercePlatform.Application.DTOs.Category;
using ECommercePlatform.Application.DTOs.Common;
using ECommercePlatform.Application.DTOs.CustomerCoupon;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommercePlatform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin, Customer")]
    public class CustomerCouponController : ControllerBase
    {
        private readonly ICustomerCouponService _customerCouponService;

        public CustomerCouponController(ICustomerCouponService customerCouponService)
        {
            _customerCouponService = customerCouponService;
        }



        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<List<GetCustomerCouponDto>>> Get([FromQuery] PaginationDto dto)
        {
            var customerCoupons = await _customerCouponService.GetAsync(dto);

            if (customerCoupons == null || customerCoupons.Count == 0)
            {
                return Ok(new
                {
                    message = "No coupons found for the requested page.",
                    data = new List<GetCategoryDto>()
                });
            }

            return Ok(customerCoupons);
        }
    }
}
