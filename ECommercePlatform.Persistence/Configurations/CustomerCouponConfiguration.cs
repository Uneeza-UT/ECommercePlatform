using ECommercePlatform.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommercePlatform.Persistence.Configurations
{
    public class CustomerCouponConfiguration : IEntityTypeConfiguration<CustomerCoupon>
    {
        public void Configure(EntityTypeBuilder<CustomerCoupon> builder)
        {
            builder.HasIndex(cc => new { cc.CouponId, cc.CustomerId })
                .IsUnique();

            builder.Property(cc => cc.GuestEmail)
                .HasMaxLength(500);

            builder.Property(cc => cc.IsUsed)
                .HasDefaultValue(false);

            builder.Property(p => p.RowVersion)
                .IsRowVersion();
        }
    }
}
