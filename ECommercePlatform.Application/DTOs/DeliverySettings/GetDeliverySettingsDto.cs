namespace ECommercePlatform.Application.DTOs.DeliverySettings
{
    public class GetDeliverySettingsDto
    {
        public int Id { get; set; }
        public decimal CashOnDeliveryFee { get; set; }
        public decimal OnlinePaymentDeliveryFee { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
