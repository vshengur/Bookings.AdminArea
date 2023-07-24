using Bookings.Domain;
using Bookings.Repositories.Contexts;
using Bookings.Repositories.Domain;
using Bookings.Repositories.Domain.Interfaces;
using Bookings.Web;

using Grpc.Core;

using MassTransit;

namespace Bookings.Storage.Services
{
    public class BookingsService : BookingsContract.BookingsContractBase
    {
        private readonly ILogger<BookingsService> _logger;
        private readonly IBookingsRepository bookingsRepository;
        private readonly IHotelsRepository hotelsRepository;
        private readonly IBus _bus;

        public BookingsService(
            IMongoDBContext dBContext,
            IBus bus,
            ILogger<BookingsService> logger,
            ILogger<BookingsRepository> bookingLogger,
            ILogger<HotelsRepository> hotelsLogger)
        {
            bookingsRepository = new BookingsRepository(dBContext, bookingLogger);
            hotelsRepository = new HotelsRepository(dBContext, hotelsLogger);

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
                bookings = await bookingsRepository.Get();
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
}