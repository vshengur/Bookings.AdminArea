using Bookings.Bus.Sagas.Events.Abstractions;
using Bookings.Domain.DTO.BookingProcess;
using Bookings.Domain;
using Bookings.Repositories.Domain;
using MassTransit;

namespace Bookings.Storage.Queues.Consumers
{
    public class BookingRequestedConsumer : IConsumer<IBookingRequested>
    {
        readonly ILogger<CreateBookingConsumer> _logger;
        private readonly BookingsRepository _bookingsRepository;
        private readonly HotelsRepository _hotelsRepository;

        public BookingRequestedConsumer(
            ILogger<CreateBookingConsumer> logger,
            BookingsRepository bookingsRepository,
            HotelsRepository hotelsRepository)
        {
            _logger = logger;
            _bookingsRepository = bookingsRepository;
            _hotelsRepository = hotelsRepository;
        }

        public async Task Consume(ConsumeContext<IBookingRequested> context)
        {
            _logger.LogInformation(message: "Creating {BookName}", context.Message.BookName);

            var newItem = new Booking()
            {
                StateId = context.Message.CorrelationId,
                BookName = context.Message.BookName,
                Category = context.Message.Category,
                Hotel = await _hotelsRepository.Get(context.Message.HotelId),
                Price = context.Message.Price
            };

            await _bookingsRepository.Create(newItem);

            await context.RespondAsync(new BookingProcessDto
            {
                BookingId = newItem.Id!,
                CorrelationId = context.Message.CorrelationId,
            });
        }
    }
}
