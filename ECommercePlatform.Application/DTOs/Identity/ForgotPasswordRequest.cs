using System.ComponentModel.DataAnnotations;

namespace ECommercePlatform.Application.DTOs.Identity
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
