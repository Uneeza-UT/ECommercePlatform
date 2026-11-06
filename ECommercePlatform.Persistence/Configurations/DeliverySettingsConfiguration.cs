using ECommercePlatform.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommercePlatform.Persistence.Configurations
{
    public class DeliverySettingsConfiguration : IEntityTypeConfiguration<DeliverySettings>
    {
        public void Configure(EntityTypeBuilder<DeliverySettings> builder)
        {
            builder.ToTable(t =>
                t.HasCheckConstraint("CK_DeliverySettings_SingleRow", "[Id] = 1"));

            builder.Property(ds => ds.CashOnDeliveryFee)
                .HasPrecision(18, 2);

            builder.Property(ds => ds.OnlinePaymentDeliveryFee)
                .HasPrecision(18, 2);


        }
    }
}
