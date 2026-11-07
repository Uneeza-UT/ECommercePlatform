using ECommercePlatform.Domain;
using ECommercePlatform.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommercePlatform.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(o => o.SubTotal)
                .HasPrecision(18, 2);

            builder.Property(o => o.DiscountAmount)
                .HasPrecision(18, 2);

            builder.Property(o => o.TotalAmount)
                .HasPrecision(18, 2);

            builder.Property(o => o.PaymentType)
                .HasConversion<string>();

            builder.Property(o => o.OrderStatus)
                .HasConversion<string>()
                .HasDefaultValue(OrderStatus.Pending);

            builder.Property(o => o.DeliveryFee)
                .HasPrecision(18, 2);
        }
    }
}
