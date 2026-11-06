using ECommercePlatform.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommercePlatform.Persistence.Configurations
{
    public class ShippingDetailsConfiguration : IEntityTypeConfiguration<ShippingDetails>
    {
        public void Configure(EntityTypeBuilder<ShippingDetails> builder)
        {
            builder.HasOne(sd => sd.Order)
                .WithOne(o => o.ShippingDetails)
                .HasForeignKey<ShippingDetails>(sd => sd.OrderId);

            builder.Property(sd => sd.FirstName)
               .HasMaxLength(100);

            builder.Property(sd => sd.LastName)
               .HasMaxLength(100);

            builder.Property(sd => sd.Email)
               .HasMaxLength(500);

            builder.Property(sd => sd.PhoneNumber)
               .HasMaxLength(100);

            builder.Property(sd => sd.City)
               .HasMaxLength(100);

            builder.Property(sd => sd.Address)
               .HasMaxLength(200);

            builder.Property(sd => sd.ShippingAddress)
               .HasMaxLength(200);

            builder.Property(sd => sd.PostalCode)
               .HasMaxLength(100);
        }
    }
}
