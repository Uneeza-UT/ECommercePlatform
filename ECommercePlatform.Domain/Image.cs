using ECommercePlatform.Domain.Common;

namespace ECommercePlatform.Domain
{
    public class Image : BaseEntity
    {
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string ContentType { get; set; }
    }
}
