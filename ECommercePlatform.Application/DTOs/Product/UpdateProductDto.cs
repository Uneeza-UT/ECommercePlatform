namespace ECommercePlatform.Application.DTOs.Product
{
    public class UpdateProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int ProductsSold { get; set; }     
        public decimal? DiscountPercentage { get; set; }
    }
}
