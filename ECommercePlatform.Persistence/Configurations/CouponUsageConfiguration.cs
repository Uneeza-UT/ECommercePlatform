using ECommercePlatform.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommercePlatform.Persistence.Configurations
{
    public class CouponUsageConfiguration : IEntityTypeConfiguration<CouponUsage>
    {
        public void Configure(EntityTypeBuilder<CouponUsage> builder)
        {
            builder.HasOne(cu => cu.Order)
                .WithOne(o => o.CouponUsage)
                .HasForeignKey<CouponUsage>(cu => cu.OrderId);
        }
    }
}
