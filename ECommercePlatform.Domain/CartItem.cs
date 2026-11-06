using ECommercePlatform.Domain.Common;

namespace ECommercePlatform.Domain
{
    public class CartItem : BaseEntity
    {
        public int CartId { get; set; }
        public Cart Cart { get; set; } = null!;

        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public int Quantity { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
