using ECommercePlatform.Application.Contracts.Services;
using ECommercePlatform.Application.DTOs.Category;
using ECommercePlatform.Application.DTOs.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommercePlatform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController] 
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }



        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<List<GetCategoryDto>>> Get([FromQuery] PaginationDto dto)
        {
            var categories = await _categoryService.GetAsync(dto);

            if (categories == null || categories.Count == 0)
            {
                return Ok(new
                {
                    message = "No categories found for the requested page.",
                    data = new List<GetCategoryDto>()
                });
            }

            return Ok(categories);
        }



        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<GetCategoryDto>> Get(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            return Ok(category);
        }






        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(409)]
        public async Task<ActionResult> Post([FromBody] CreateCategoryDto dto)
        {
            var response = await _categoryService.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = response });
        }





        [Authorize(Roles = "Admin")]
        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        public async Task<ActionResult> Put(int id, [FromBody] UpdateCategoryDto dto)
        {
            await _categoryService.UpdateAsync(id, dto);
            return NoContent();
        }





        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Delete(int id)
        {
            await _categoryService.DeleteAsync(id);
            return NoContent();
        }
    }
}
