using ECommercePlatform.Application.DTOs.Product;

namespace ECommercePlatform.Application.DTOs.Category
{
    public class GetCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<GetProductsDto>? Products { get; set; } = new List<GetProductsDto>();
    }
}
