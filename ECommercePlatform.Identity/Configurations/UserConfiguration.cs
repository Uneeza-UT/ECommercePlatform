using ECommercePlatform.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommercePlatform.Identity.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            var passwordHasher = new PasswordHasher<ApplicationUser>();

            builder.HasData(
                new ApplicationUser
                {
                    Id = "d40a6fb5-9e72-4184-8053-bca6e4f937d8",
                    Email = "admin@example.com",
                    NormalizedEmail = "ADMIN@EXAMPLE.COM",
                    FirstName = "System",
                    LastName = "Admin",
                    UserName = "admin@example.com",
                    NormalizedUserName = "ADMIN@EXAMPLE.COM",
                    PasswordHash = passwordHasher.HashPassword(null, "Admin1234!"),
                    EmailConfirmed = true
                },

                new ApplicationUser
                {
                    Id = "e51b70c6-b094-43a6-9164-dec8061959fa",
                    Email = "customer@example.com",
                    NormalizedEmail = "CUSTOMER@EXAMPLE.COM",
                    FirstName = "Test",
                    LastName = "Customer",
                    UserName = "customer@example.com",
                    NormalizedUserName = "CUSTOMER@EXAMPLE.COM",
                    PasswordHash = passwordHasher.HashPassword(null, "Customer1234!"),
                    EmailConfirmed = true
                }
            );
        }
    }
}
