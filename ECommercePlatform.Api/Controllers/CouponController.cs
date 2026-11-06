using ECommercePlatform.Application.Contracts.Services;
using ECommercePlatform.Application.DTOs.Common;
using ECommercePlatform.Application.DTOs.Coupon;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommercePlatform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CouponController : ControllerBase
    {
        private readonly ICouponService _couponService;

        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }



        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<List<GetCouponDto>>> Get([FromQuery] PaginationDto dto)
        {
            var coupons = await _couponService.GetAsync(dto);

            if (coupons == null || coupons.Count == 0)
            {
                return Ok(new
                {
                    message = "No coupons found for the requested page.",
                    data = new List<GetCouponDto>()
                });
            }

            return Ok(coupons);
        }



        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<GetCouponDto>> Get(int id)
        {
            var coupon = await _couponService.GetByIdAsync(id);
            return Ok(coupon);
        }






        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(409)]
        public async Task<ActionResult> Post([FromBody] CreateCouponDto dto)
        {
            var response = await _couponService.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = response });
        }






        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        public async Task<ActionResult> Put(int id, [FromBody] UpdateCouponDto dto)
        {
            await _couponService.UpdateAsync(id, dto);
            return NoContent();
        }




        [HttpPatch("change-status")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Patch(int id, [FromBody] ChangeCouponStatusDto dto)
        {
            await _couponService.ChangeStatusAsync(id, dto);
            return NoContent();
        }





        [HttpPatch("assign-customer")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        public async Task<ActionResult> Patch(int id, [FromBody] AssignCouponDto dto)
        {
            await _couponService.AssignToCustomerAsync(id, dto);
            return NoContent();
        }






        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Delete(int id)
        {
            await _couponService.DeleteAsync(id);
            return NoContent();
        }
    }
}
