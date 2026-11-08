using ECommercePlatform.Application.Contracts.Services;
using ECommercePlatform.Application.DTOs.Common;
using ECommercePlatform.Application.DTOs.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommercePlatform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }



        [HttpGet("category/{categoryId}")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<List<GetProductsDto>>> GetByCategory(int categoryId, [FromQuery] PaginationDto dto)
        {
            var products = await _productService.GetAsync(categoryId, dto);

            if (products == null || products.Count == 0)
            {
                return Ok(new
                {
                    message = "No products found for the requested page.",
                    data = new List<GetProductsDto>()
                });
            }

            return Ok(products);
        }




        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<GetProductsDto>> Get(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            return Ok(product);
        }




        [Authorize(Roles = "Admin")]
        [HttpPost("category/{categoryId}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(409)]
        public async Task<ActionResult> Post(int categoryId, [FromForm] CreateProductDto dto)
        {
            var response = await _productService.CreateAsync(categoryId, dto);
            return Created("", response);
        }





        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        public async Task<ActionResult> Put(int id, [FromForm] UpdateProductDto dto)
        {
            await _productService.UpdateAsync(id, dto);
            return NoContent();
        }






        [Authorize(Roles = "Admin")]
        [HttpPut("restock/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Restock(int id, [FromBody] RestockProductDto dto)
        {
            await _productService.RestockAsync(id, dto);
            return NoContent();
        }






        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Delete(int id)
        {
            await _productService.DeleteAsync(id);
            return NoContent();
        }




        [HttpGet("search")]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<List<GetProductsDto>>> Search(
            [FromQuery] SearchDto searchDto, [FromQuery] PaginationDto paginationDto)
        {
            var products = await _productService.SearchAsync(searchDto, paginationDto);
            return products;
        }



        [HttpGet("sort")]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<List<GetProductsDto>>> Sort(
            [FromQuery] SortDto sortDto, [FromQuery] PaginationDto paginationDto)
        {
            var products = await _productService.SortAsync(sortDto, paginationDto);
            return products;
        }



        [HttpGet("filter")]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<List<GetProductsDto>>> Filter(
            [FromQuery] FilterDto filterDto, [FromQuery] PaginationDto paginationDto)
        {
            var products = await _productService.FilterAsync(filterDto, paginationDto);
            return products;
        }
    }
}
