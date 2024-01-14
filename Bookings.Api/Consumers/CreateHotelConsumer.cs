using Bookings.Bus.Queues.Messages;
using Bookings.Domain;
using Bookings.Repositories.Domain.Interfaces;

using MassTransit;

namespace Bookings.Api.Consumers;

public class CreateHotelConsumer : IConsumer<CreateHotelMessage>
{
    readonly ILogger<CreateHotelConsumer> _logger;
    private readonly IHotelsRepository _hotelsRepository;

    public CreateHotelConsumer(
        ILogger<CreateHotelConsumer> logger,
        IHotelsRepository hotelsRepository)
    {
        _logger = logger;
        _hotelsRepository = hotelsRepository;
    }

    public async Task Consume(ConsumeContext<CreateHotelMessage> context)
    {
        _logger.LogInformation(message: "Creating {HotelName}", context.Message.Name);

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

        await _hotelsRepository.Create(newItem);
        return;
    }
}
