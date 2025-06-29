using Bookings.Bus.Queues.Messages;
using Bookings.Domain;
using Bookings.Repositories.Domain.Interfaces;

using MassTransit;

namespace Bookings.Hotels.Consumers;

public class CreateRoomConsumer(
    ILogger<CreateRoomConsumer> logger,
    IHotelsRepository hotelsRepository,
    IRoomsRepository roomsRepository)
    : IConsumer<CreateRoomMessage>
{
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