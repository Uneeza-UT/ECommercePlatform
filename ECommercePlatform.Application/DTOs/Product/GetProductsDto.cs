namespace ECommercePlatform.Application.DTOs.Product
{
    public class GetProductsDto
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int ProductsSold { get; set; }    
        public decimal? DiscountPercentage { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
