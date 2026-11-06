using ECommercePlatform.Domain.Common;

namespace ECommercePlatform.Domain
{
    public class DeliverySettings : BaseEntity
    {
        public decimal CashOnDeliveryFee { get; set; }
        public decimal OnlinePaymentDeliveryFee { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
