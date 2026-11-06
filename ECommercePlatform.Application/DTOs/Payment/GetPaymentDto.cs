using ECommercePlatform.Domain.Enums;

namespace ECommercePlatform.Application.DTOs.Payment
{
    public class GetPaymentDto
    {
        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; }
        public string? TransactionReference { get; set; }
        public DateTime? PaidAt { get; set; }
    }
}
