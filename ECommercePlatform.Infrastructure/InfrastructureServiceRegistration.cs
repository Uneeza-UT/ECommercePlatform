using ECommercePlatform.Application.Contracts.Email;
using ECommercePlatform.Application.Contracts.Services;
using ECommercePlatform.Application.Models.Email;
using ECommercePlatform.Infrastructure.EmailService;
using ECommercePlatform.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Resend;

namespace ECommercePlatform.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

            services.AddHttpClient<ResendClient>();
            services.Configure<ResendClientOptions>(options =>
            {
                options.ApiToken = configuration.GetValue<string>("EmailSettings:ApiKey");
            });


            services.AddTransient<IResend, ResendClient>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddScoped<ICartItemService, CartItemService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICouponService, CouponService>();
            services.AddScoped<ICustomerCouponService, CustomerCouponService>();
            services.AddScoped<IDeliverySettingsService, DeliverySettingsService>();
            services.AddScoped<IFileStorageService, FileStorageService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddHostedService<CouponStatusChangeService>();


            return services;
        }
    }
}
