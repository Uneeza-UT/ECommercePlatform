using ECommercePlatform.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommercePlatform.Persistence.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Name)
                .HasMaxLength(100);

            builder.Property(p => p.Price)
                .HasPrecision(18, 2);

            builder.Property(p => p.ProductsSold)
                .HasDefaultValue(0);

            builder.Property(p => p.DiscountPercentage)
                .HasPrecision(5, 2);

            builder.Property(p => p.IsDeleted)
                .HasDefaultValue(false);

            builder.Property(p => p.RowVersion)
                .IsRowVersion();
        }
    }
}
