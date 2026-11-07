using ECommercePlatform.Application.Contracts.Services;
using ECommercePlatform.Application.DTOs.CartItem;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommercePlatform.Api.Controllers
{
    [Route("api/cart/items/{productId:int}")]
    [ApiController]
    [Authorize(Roles = "Admin, Customer")]
    public class CartItemController : ControllerBase
    {
        private readonly ICartItemService _cartItemService;

        public CartItemController(ICartItemService cartItemService)
        {
            _cartItemService = cartItemService;
        }



        [HttpPost("add-product")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Post(int productId, [FromBody] AddCartItemDto dto)
        {
            var cartItems = await _cartItemService.AddItemToCartAsync(productId, dto);
            return Created("", cartItems);
        }




        [HttpPatch("items/increase-quantity")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> IncreaseQuantity(int productId)
        {
            await _cartItemService.UpdateQuantityAsync(productId, true);
            return NoContent();
        }



        [HttpPatch("items/decrease-quantity")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> DecreaseQuantity(int productId)
        {
            await _cartItemService.UpdateQuantityAsync(productId, false);
            return NoContent();
        }



        [HttpDelete("items/remove")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> RemoveItem(int productId)
        {
            await _cartItemService.RemoveItemFromCartAsync(productId);
            return NoContent();
        }
    }
}
