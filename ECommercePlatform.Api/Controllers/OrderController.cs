using ECommercePlatform.Application.Contracts.Services;
using ECommercePlatform.Application.DTOs.Common;
using ECommercePlatform.Application.DTOs.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommercePlatform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }


        [Authorize(Roles = "Admin, Customer")]
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<List<GetOrderDto>>> Get([FromQuery] PaginationDto dto)
        {
            var orders = await _orderService.GetAsync(dto);

            if (orders == null || orders.Count == 0)
            {
                return Ok(new
                {
                    message = "No orders found for the requested page.",
                    data = new List<GetOrderDto>()
                });
            }

            return Ok(orders);
        }




        [Authorize(Roles = "Admin, Customer")]
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<GetOrderDto>> Get(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            return Ok(order);
        }






        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Post([FromForm] CreateOrderDto dto)
        {
            var response = await _orderService.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = response });
        }





        [Authorize(Roles = "Admin")]
        [HttpPatch("change-status")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Patch(int id, [FromBody] ChangeOrderStatusDto dto)
        {
            await _orderService.ChangeOrderStatusAsync(id, dto);
            return NoContent();
        }
    }
}
