using ECommercePlatform.Application.Contracts.Identity;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ECommercePlatform.Identity.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? UserId =>
            _httpContextAccessor.HttpContext?
            .User?
            .FindFirst("UserId")?
            .Value;


        public string? UserRole =>
            _httpContextAccessor.HttpContext?
            .User?
            .FindFirst(ClaimTypes.Role)?
            .Value;



        public bool IsAuthenticated =>
            _httpContextAccessor.HttpContext?
            .User
            .Identity?
            .IsAuthenticated ?? false;
    }
}
