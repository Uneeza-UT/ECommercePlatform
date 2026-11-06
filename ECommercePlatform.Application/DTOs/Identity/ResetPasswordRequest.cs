using System.ComponentModel.DataAnnotations;

namespace ECommercePlatform.Application.DTOs.Identity
{
    public class ResetPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        [MinLength(8)]
        public string NewPassword { get; set; }
    }
}
