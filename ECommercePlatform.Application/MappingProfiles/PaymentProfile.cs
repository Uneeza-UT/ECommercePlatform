using AutoMapper;
using ECommercePlatform.Application.DTOs.Payment;
using ECommercePlatform.Domain;

namespace ECommercePlatform.Application.MappingProfiles
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<Payment, GetPaymentDto>();
        }
    }
}
