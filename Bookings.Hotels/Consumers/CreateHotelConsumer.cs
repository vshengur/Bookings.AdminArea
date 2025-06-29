using Bookings.Bus.Queues.Messages;
using Bookings.Domain;
using Bookings.Domain.Repositories;

using MassTransit;

namespace Bookings.Hotels.Consumers;

public class CreateHotelConsumer(
    ILogger<CreateHotelConsumer> logger,
    IHotelsRepository hotelsRepository)
    : IConsumer<CreateHotelMessage>
{
    public async Task Consume(ConsumeContext<CreateHotelMessage> context)
    {
        logger.LogInformation(message: "Creating {HotelName}", context.Message.Name);

        var newItem = new Hotel()
        {
            Name = context.Message.Name,
            City = context.Message.City,
            Country = context.Message.Country,
            Stars = context.Message.Stars,
            Rate = context.Message.Rate,
            LocationX = context.Message.LocationX,
            LocationY = context.Message.LocationY,
        };

        await hotelsRepository.Create(newItem);
        return;
    }
}
