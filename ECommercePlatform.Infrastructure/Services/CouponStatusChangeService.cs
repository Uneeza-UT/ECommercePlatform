using ECommercePlatform.Application.Contracts.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommercePlatform.Infrastructure.Services
{
    public class CouponStatusChangeService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<CouponStatusChangeService> _logger;

        public CouponStatusChangeService(IServiceScopeFactory scopeFactory, ILogger<CouponStatusChangeService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }


        // Checks the start date of a coupon every hour and mark its status as 'Active'
        // Checks the expiry date of a coupon every hour and mark its status as 'Expired' 
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();

                    var couponRepository = scope.ServiceProvider
                        .GetRequiredService<ICouponRepository>();

                    await couponRepository.ExpireCouponsAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError("An error occurred while expiring coupons.");
                }

                await Task.Delay(TimeSpan.FromMinutes(15), stoppingToken);
            }
        }
    }
}
