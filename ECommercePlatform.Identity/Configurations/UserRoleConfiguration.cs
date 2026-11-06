using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommercePlatform.Identity.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasData(
                new IdentityUserRole<string>
                {
                    UserId = "d40a6fb5-9e72-4184-8053-bca6e4f937d8",
                    RoleId = "a17f3c82-6b49-4e51-9d20-8f73b1c604a5" // Admin.
                },

                new IdentityUserRole<string>
                {
                    UserId = "e51b70c6-b094-43a6-9164-dec8061959fa",
                    RoleId = "n54f5ea4-4f62-4073-7c50-ab95d3e826c7" // Customer.
                }
            );
        }
    }
}
