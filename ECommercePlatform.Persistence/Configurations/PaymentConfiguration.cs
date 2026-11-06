using ECommercePlatform.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommercePlatform.Persistence.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasOne(p => p.Order)
                .WithOne(o => o.Payment)
                .HasForeignKey<Payment>(p => p.OrderId);

            builder.Property(p => p.Amount)
               .HasPrecision(18, 2);

            builder.Property(p => p.Status)
                .HasConversion<string>();

            builder.Property(p => p.TransactionReference)
               .HasMaxLength(200);
        }
    }
}
