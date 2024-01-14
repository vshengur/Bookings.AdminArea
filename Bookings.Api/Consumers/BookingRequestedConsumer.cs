using Bookings.Bus.Sagas.Events.Abstractions;
using Bookings.Domain;
using Bookings.Domain.Dto.BookingProcess;
using Bookings.Repositories.Domain.Interfaces;

using MassTransit;

namespace Bookings.Api.Consumers;

public class BookingRequestedConsumer(
    ILogger<BookingRequestedConsumer> logger,
    IBookingsRepository bookingsRepository,
    IRoomsRepository roomsRepository)
    : IConsumer<IBookingRequested>
{
    public async Task Consume(ConsumeContext<IBookingRequested> context)
    {
        logger.LogInformation(message: "Creating {BookName}", context.Message.BookName);

        var room = await roomsRepository.Get(context.Message.RoomId);

        Booking newItem = new ()
        {
            StateId = context.Message.CorrelationId,
            BookName = context.Message.BookName,
            Category = context.Message.Category,
            Room = room,
            Price = context.Message.Price
        };

        await bookingsRepository.Create(newItem);

        await context.RespondAsync(new BookingProcessDto
        {
            BookingId = newItem.Id.ToString() !,
            StateId = context.Message.StateId
        });
    }
}
