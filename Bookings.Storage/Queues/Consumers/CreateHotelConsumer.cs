using Bookings.Domain.Queues.Messages;
using Bookings.Domain;
using Bookings.Repositories.Contexts;
using Bookings.Repositories.Domain;
using MassTransit;

namespace Bookings.Storage.Queues.Consumers
{
    public class CreateHotelConsumer : IConsumer<CreateHotelMessage>
    {
        readonly ILogger<CreateHotelConsumer> _logger;
        private readonly BookingsRepository bookingsRepository;
        private readonly HotelsRepository hotelsRepository;

        public CreateHotelConsumer(ILogger<CreateHotelConsumer> logger,
            IMongoDBContext dBContext)
        {
            _logger = logger;
            bookingsRepository = new BookingsRepository(dBContext);
            hotelsRepository = new HotelsRepository(dBContext);
        }

        public async Task Consume(ConsumeContext<CreateHotelMessage> context)
        {
            _logger.LogInformation(message: "Creating {BookName}", context.Message.Name);

            var newItem = new Hotel()
            {
                Name = context.Message.Name,
                City = context.Message.City,
                Stars = context.Message.Stars,
                RoomsCount = context.Message.RoomsCount,
                LocationX = context.Message.LocationX,
                LocationY = context.Message.LocationY,
            };

            await hotelsRepository.Create(newItem);
            return;
        }
    }
}
