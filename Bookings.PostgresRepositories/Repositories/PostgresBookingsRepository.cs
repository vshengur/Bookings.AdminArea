using Bookings.Domain;
using Bookings.Domain.Repositories;
using Bookings.PostgresRepositories.Contexts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Bookings.PostgresRepositories.Repositories;

public class PostgresBookingsRepository : BasePostgresRepository<Booking>, IBookingsRepository
{
    public PostgresBookingsRepository(
        BookingsDbContext context,
        ILogger<PostgresBookingsRepository> logger) : base(context, logger)
    {
    }

    public async Task<List<Booking>> GetByHotelId(string hotelId)
    {
        return await _dbSet
            .Where(b => b.HotelId == hotelId)
            .OrderBy(b => b.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Booking>> GetByRoomId(string roomId)
    {
        return await _dbSet
            .Where(b => b.RoomId == roomId)
            .OrderBy(b => b.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Booking>> GetByDateRange(DateTime checkIn, DateTime checkOut)
    {
        return await _dbSet
            .Where(b => b.CheckInDate <= DateOnly.FromDateTime(checkIn) && b.CheckOutDate >= DateOnly.FromDateTime(checkOut))
            .OrderBy(b => b.CheckInDate)
            .ToListAsync();
    }
} 