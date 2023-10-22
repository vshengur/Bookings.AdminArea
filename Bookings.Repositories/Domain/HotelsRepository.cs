using Bookings.Domain;
using Bookings.Repositories.Contexts;
using Bookings.Repositories.Domain.Interfaces;

using Microsoft.Extensions.Logging;

namespace Bookings.Repositories.Domain;

public class HotelsRepository : BaseRepository<Hotel>, IHotelsRepository
{
    public HotelsRepository(IMongoDBContext context, ILogger<HotelsRepository> logger)
        : base(context, logger)
    {
    }
}
