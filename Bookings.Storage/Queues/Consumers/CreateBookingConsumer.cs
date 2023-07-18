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
        private readonly BookingsRepository bookingsRepository;
        private readonly HotelsRepository hotelsRepository;

        public CreateBookingConsumer(ILogger<CreateBookingConsumer> logger,
            IMongoDBContext dBContext)
        {
            _logger = logger;
            bookingsRepository = new BookingsRepository(dBContext);
            hotelsRepository = new HotelsRepository(dBContext);
        }

        public async Task Consume(ConsumeContext<CreateBookingMessage> context)
        {
            _logger.LogInformation(message: "Creating {BookName}", context.Message.BookName);

            var newItem = new Booking()
            {
                BookName = context.Message.BookName,
                Category = context.Message.Category,
                Hotel = await hotelsRepository.Get(context.Message.HotelId),
                Price = context.Message.Price
            };

            await bookingsRepository.Create(newItem);
            return;
        }
    }
}
