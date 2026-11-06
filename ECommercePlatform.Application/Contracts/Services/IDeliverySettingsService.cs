using ECommercePlatform.Application.DTOs.DeliverySettings;

namespace ECommercePlatform.Application.Contracts.Services
{
    public interface IDeliverySettingsService
    {
        Task<GetDeliverySettingsDto> GetAsync();
        Task<int> CreateAsync(CreateDeliverySettingsDto dto);
        Task UpdateAsync(int id, UpdateDeliverySettingsDto dto);
    }
}
