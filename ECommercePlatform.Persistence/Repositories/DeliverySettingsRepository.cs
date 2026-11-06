using ECommercePlatform.Application.Contracts.Persistence;
using ECommercePlatform.Domain;
using ECommercePlatform.Persistence.DatabaseContext;

namespace ECommercePlatform.Persistence.Repositories
{
    public class DeliverySettingsRepository : GenericRepository<DeliverySettings>, IDeliverySettingsRepository
    {
        public DeliverySettingsRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
