using ECommercePlatform.Domain;
using ECommercePlatform.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommercePlatform.Persistence.Configurations
{
    public class CouponConfiguration : IEntityTypeConfiguration<Coupon>
    {
        public void Configure(EntityTypeBuilder<Coupon> builder)
        {
            builder.HasIndex(c => c.Code)
                .IsUnique();

            builder.Property(c => c.Code)
                .HasMaxLength(100);

            builder.Property(c => c.CouponType)
                .HasConversion<string>();

            builder.Property(c => c.CouponStatus)
                .HasConversion<string>()
                .HasDefaultValue(CouponStatus.Active);

            builder.Property(c => c.AssignmentType)
                .HasConversion<string>();

            builder.Property(c => c.DiscountPercentage)
                .HasPrecision(5, 2);

            builder.Property(c => c.DiscountAmount)
                .HasPrecision(18, 2);

            builder.Property(c => c.MinimumOrderAmount)
                .HasPrecision(18, 2);

            builder.Property(c => c.MaximumDiscount)
                .HasPrecision(18, 2);

            builder.Property(c => c.IsDeleted)
                .HasDefaultValue(false);


            builder.HasData(
                new Coupon
                {
                    Id = 1,
                    Code = "WELCOME2026",
                    CouponType = CouponType.Percentage,
                    CouponStatus = CouponStatus.Active,
                    AssignmentType = CouponAssignmentType.CustomerSpecific,
                    DiscountPercentage = 10,
                    DiscountAmount = null,
                    MinimumOrderAmount = 0,
                    MaximumDiscount = null,
                    ValidityDays = 30,
                    StartsAt = null,
                    ExpiryDate = null
                },
                new Coupon
                {
                    Id = 2,
                    Code = "FIRSTORDER",
                    CouponType = CouponType.FixedAmount,
                    CouponStatus = CouponStatus.Active,
                    AssignmentType = CouponAssignmentType.Public,
                    DiscountPercentage = null,
                    DiscountAmount = 500,
                    MinimumOrderAmount = 0,
                    MaximumDiscount = null,
                    ValidityDays = 30,
                    StartsAt = null,
                    ExpiryDate = null
                }
            );
        }
    }
}
