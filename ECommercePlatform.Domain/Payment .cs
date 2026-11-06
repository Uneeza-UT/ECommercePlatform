using ECommercePlatform.Domain.Enums;

namespace ECommercePlatform.Domain
{
    public class Payment
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order? Order { get; set; }

        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; }

        public string? TransactionReference { get; set; }
        public DateTime? PaidAt { get; set; }
    }
}
