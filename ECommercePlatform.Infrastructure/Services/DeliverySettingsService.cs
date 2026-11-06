using AutoMapper;
using ECommercePlatform.Application.Contracts.Persistence;
using ECommercePlatform.Application.Contracts.Services;
using ECommercePlatform.Application.DTOs.DeliverySettings;
using ECommercePlatform.Application.Exceptions;
using ECommercePlatform.Application.Validations.DeliverySettings;
using ECommercePlatform.Domain;

namespace ECommercePlatform.Infrastructure.Services
{
    public class DeliverySettingsService : IDeliverySettingsService
    {
        private readonly IMapper _mapper;
        private readonly IDeliverySettingsRepository _deliverySettingsRepository;

        public DeliverySettingsService(IMapper mapper, IDeliverySettingsRepository deliverySettingsRepository)
        {
            _mapper = mapper;
            _deliverySettingsRepository = deliverySettingsRepository;
        }


        public async Task<GetDeliverySettingsDto> GetAsync()
        {
            var deliverySettings = await _deliverySettingsRepository.GetByIdAsync(1);
            return _mapper.Map<GetDeliverySettingsDto>(deliverySettings);
        }



        public async Task<int> CreateAsync(CreateDeliverySettingsDto dto)
        {
            // validate the dto
            var validator = new CreateDeliverySettingsValidator();
            var validationResult = await validator.ValidateAsync(dto);
            
            if (!validationResult.IsValid)
            {
                throw new BadRequestException("Invalid delivery settings data. ", validationResult);
            }


            var settings = new DeliverySettings
            {
                Id = 1,
                CashOnDeliveryFee = dto.CashOnDeliveryFee,
                OnlinePaymentDeliveryFee = dto.OnlinePaymentDeliveryFee
            };

            await _deliverySettingsRepository.CreateAsync(settings);

            return settings.Id;
        }

        

        public async Task UpdateAsync(int id, UpdateDeliverySettingsDto dto)
        {
            var deliverySettings = await _deliverySettingsRepository.GetByIdAsync(id);

            if (deliverySettings == null)
            {
                throw new NotFoundException(nameof(DeliverySettings), id);
            }

            // validate the dto
            var validator = new UpdateDeliverySettingsValidator();
            var validationResult = await validator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                throw new BadRequestException("Invalid delivery settings data. ", validationResult);
            }
          

            _mapper.Map(dto, deliverySettings);
            deliverySettings.UpdatedAt = DateTime.UtcNow;

            await _deliverySettingsRepository.SaveChangesAsync();
        }
    }
}
