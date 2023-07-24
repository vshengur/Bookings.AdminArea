using Bookings.Domain;
using Bookings.Domain.Queues.Messages;
using Bookings.Repositories.Contexts;
using Bookings.Repositories.Domain;

using MassTransit;

namespace Bookings.Storage.Queues.Consumers
{
    public class CreateBookingConsumer : IConsumer<CreateBookingMessage>
    {
        readonly ILogger<CreateBookingConsumer> _logger;
        private readonly BookingsRepository _bookingsRepository;
        private readonly HotelsRepository _hotelsRepository;

        public CreateBookingConsumer(
            ILogger<CreateBookingConsumer> logger,
            BookingsRepository bookingsRepository,
            HotelsRepository hotelsRepository)
        {
            _logger = logger;
            _bookingsRepository = bookingsRepository;
            _hotelsRepository = hotelsRepository;
        }

        public async Task Consume(ConsumeContext<CreateBookingMessage> context)
        {
            _logger.LogInformation(message: "Creating {BookName}", context.Message.BookName);

            var newItem = new Booking()
            {
                BookName = context.Message.BookName,
                Category = context.Message.Category,
                Hotel = await _hotelsRepository.Get(context.Message.HotelId),
                Price = context.Message.Price
            };

            await _bookingsRepository.Create(newItem);
            return;
        }
    }
}
