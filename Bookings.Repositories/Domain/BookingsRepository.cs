using Bookings.Domain;
using Bookings.Repositories.Contexts;
using Bookings.Repositories.Domain.Interfaces;

using Microsoft.Extensions.Logging;

namespace Bookings.Repositories.Domain
{
    public class BookingsRepository : BaseRepository<Booking>, IBookingsRepository
    {
        public BookingsRepository(IMongoDBContext context, ILogger<BookingsRepository> logger) 
            : base(context, logger)
        {
        }
    }
}
