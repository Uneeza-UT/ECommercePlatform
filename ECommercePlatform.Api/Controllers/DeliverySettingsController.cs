using ECommercePlatform.Application.Contracts.Services;
using ECommercePlatform.Application.DTOs.DeliverySettings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommercePlatform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class DeliverySettingsController : ControllerBase
    {
        private readonly IDeliverySettingsService _deliverySettingsService;

        public DeliverySettingsController(IDeliverySettingsService deliverySettingsService)
        {
            _deliverySettingsService = deliverySettingsService;
        }




        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<GetDeliverySettingsDto>> Get()
        {
            var deliverySettings = await _deliverySettingsService.GetAsync();
            return Ok(deliverySettings);
        }






        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        public async Task<ActionResult> Post([FromBody] CreateDeliverySettingsDto dto)
        {
            var id = await _deliverySettingsService.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), null, new { Id = id });
        }





        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Put(int id, [FromBody] UpdateDeliverySettingsDto dto)
        {
            await _deliverySettingsService.UpdateAsync(id, dto);
            return NoContent();
        }
    }
}
