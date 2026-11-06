using ECommercePlatform.Application.Contracts.Services;
using ECommercePlatform.Application.DTOs.Cart;
using ECommercePlatform.Application.DTOs.CartItem;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommercePlatform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin, Customer")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }



        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<GetCartDto>> Get()
        {
            var cart = await _cartService.GetAsync();
            return Ok(cart);
        }





        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Put([FromBody] AddCartItemDto dto)
        {
            await _cartService.AddItemToCartAsync(dto);
            return NoContent();
        }
    }
}
