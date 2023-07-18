using Bookings.Domain;
using Bookings.Repositories.Contexts;
using Bookings.Repositories.Domain.Interfaces;

namespace Bookings.Repositories.Domain
{
    public class BookingsRepository : BaseRepository<Booking>, IBookingsRepository
    {
        public BookingsRepository(IMongoDBContext context) 
            : base(context)
        {
        }
    }
}
