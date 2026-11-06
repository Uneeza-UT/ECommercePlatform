using ECommercePlatform.Domain.Common;

namespace ECommercePlatform.Domain
{
    public class Cart : BaseEntity
    {
        public string UserId { get; set; }
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
