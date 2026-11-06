namespace ECommercePlatform.Application.Contracts.Identity
{
    public interface ICurrentUserService
    {
        public string? UserId { get; }
        public string? UserRole { get; }
        public bool IsAuthenticated { get; }
    }
}
