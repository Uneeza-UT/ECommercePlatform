using ECommercePlatform.Application.Models.Email;

namespace ECommercePlatform.Application.Contracts.Email
{
    public interface IEmailSender
    {
        Task<bool> SendEmailAsync(EmailMessageData emailMessageData);
    }
}
