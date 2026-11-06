using ECommercePlatform.Application.Contracts.Services;
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
