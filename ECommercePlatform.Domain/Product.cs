using ECommercePlatform.Domain.Common;

namespace ECommercePlatform.Domain
{
    public class Product : BaseEntity
    {
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int ProductsSold { get; set; }     
        public decimal? DiscountPercentage { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public byte[] RowVersion { get; set; } = null!;
    }
}
