using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommercePlatform.Identity.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                {
                    Id = "a17f3c82-6b49-4e51-9d20-8f73b1c604a5",
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },

                new IdentityRole
                {
                    Id = "n54f5ea4-4f62-4073-7c50-ab95d3e826c7",
                    Name = "Customer",
                    NormalizedName = "CUSTOMER"
                }
            );
        }
    }
}
