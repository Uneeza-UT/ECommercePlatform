using ECommercePlatform.Domain.Common;

namespace ECommercePlatform.Domain
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
