using Bookings.Domain;
using Bookings.Domain.Repositories;
using Bookings.PostgresRepositories.Contexts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Bookings.PostgresRepositories.Repositories;

public class PostgresBookingStateRepository : BasePostgresRepository<BookingStateEntity>, IBookingStateRepository
{
    public PostgresBookingStateRepository(
        BookingsDbContext context,
        ILogger<PostgresBookingStateRepository> logger) : base(context, logger)
    {
    }

    public async Task<BookingStateEntity?> GetByBookingId(string bookingId)
    {
        return await _dbSet
            .Where(bs => bs.BookingId == bookingId)
            .OrderByDescending(bs => bs.CreatedAt)
            .FirstOrDefaultAsync();
    }

    public async Task<List<BookingStateEntity>> GetByState(BookingStatus state)
    {
        return await _dbSet
            .Where(bs => bs.Status == state)
            .OrderBy(bs => bs.CreatedAt)
            .ToListAsync();
    }
} 