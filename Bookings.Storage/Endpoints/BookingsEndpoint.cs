using Bookings.Contracts;
using Bookings.Domain;
using Bookings.Repositories.Contexts;
using Bookings.Repositories.Domain;
using Bookings.Repositories.Domain.Interfaces;

using Grpc.Core;

using MassTransit;

namespace Bookings.Storage.Services;

public class BookingsEndpoint : BookingsContract.BookingsContractBase
{
    private readonly ILogger<BookingsEndpoint> _logger;
    private readonly IBookingsRepository bookingsRepository;
    private readonly IBus _bus;

    public BookingsEndpoint(
        IMongoDBContext dBContext,
        IBus bus,
        ILogger<BookingsEndpoint> logger,
        ILogger<BookingsRepository> bookingLogger)
    {
        bookingsRepository = new BookingsRepository(dBContext, bookingLogger);

        _logger = logger;
        _bus = bus;
    }

    public override async Task<BookingsResponse?> GetBookings(BookingsRequest request, ServerCallContext context)
    {
        IEnumerable<Booking> bookings;

        if (!string.IsNullOrWhiteSpace(request.Id))
        {
            bookings = new[] { await bookingsRepository.Get(request.Id) };
        }
        else
        {
            bookings = await bookingsRepository.Get(request.Page, request.Count);
        }

        if (bookings == null)
        {
            return null;
        }

        BookingsResponse response = new();
        response.Bookings.AddRange(bookings.Select(_ =>
            new BookingItem
            {
                Id = _.Id,
                BookName = _.BookName,
                Category = _.Category,
                HotelId = _.Hotel.Id,
                Price = _.Price
            }));

        return response;
    }
}