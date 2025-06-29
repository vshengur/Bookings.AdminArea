using Bookings.Domain;
using Bookings.Domain.Repositories;
using Bookings.Infrastructure.Documents;
using Bookings.Infrastructure.Mappers;
using Bookings.Repositories.Contexts;

using Microsoft.Extensions.Logging;

namespace Bookings.Repositories.Domain;

public class BookingsRepository(IMongoDBContext context, IDocumentMapper<Booking, BookingDocument> mapper, ILogger<BookingsRepository> logger) 
    : BaseRepository<Booking, BookingDocument>(context, mapper, logger), IBookingsRepository
{
}
