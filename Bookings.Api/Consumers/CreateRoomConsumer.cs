using Bookings.Bus.Queues.Messages;
using Bookings.Domain;
using Bookings.Repositories.Domain.Interfaces;

using MassTransit;

namespace Bookings.Api.Consumers;

public class CreateRoomConsumer : IConsumer<CreateRoomMessage>
{
    readonly ILogger<CreateHotelConsumer> logger;
    private readonly IHotelsRepository hotelsRepository;
    private readonly IRoomsRepository roomsRepository;

    public CreateRoomConsumer(
        ILogger<CreateHotelConsumer> logger,
        IHotelsRepository hotelsRepository,
        IRoomsRepository roomsRepository)
    {
        this.logger = logger;
        this.hotelsRepository = hotelsRepository;
        this.roomsRepository = roomsRepository;
    }

    public async Task Consume(ConsumeContext<CreateRoomMessage> context)
    {
        var hotel = await hotelsRepository.Get(context.Message.HotelId);

        logger.LogInformation(message: "Creating {RoomName} at {HotelName}", context.Message.Name, hotel.Name);

        var newItem = new Room()
        {
            Name = context.Message.Name,
            AdditionalFreeKids = context.Message.AdditionalFreeKids,
            Hotel = hotel,
            MaxPersons = context.Message.MaxPersons,
        };

        await roomsRepository.Create(newItem);
        return;
    }
}