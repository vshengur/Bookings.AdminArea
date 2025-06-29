using Bookings.Domain;
using Bookings.Domain.Repositories;
using Bookings.Infrastructure.Documents;
using Bookings.Infrastructure.Mappers;
using Bookings.Repositories.Contexts;

using Microsoft.Extensions.Logging;

namespace Bookings.Repositories.Domain;

public class HotelsRepository(IMongoDBContext context, IDocumentMapper<Hotel, HotelDocument> mapper, ILogger<HotelsRepository> logger)
    : BaseRepository<Hotel, HotelDocument>(context, mapper, logger), IHotelsRepository
{
}
