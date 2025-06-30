using Bookings.Bus.Sagas.Events.Abstractions;
using Bookings.Bus.Sagas.Events.Realizations;
using Bookings.Domain.Dto;
using Bookings.Domain.Dto.BookingProcess;

using MassTransit;

namespace Bookings.Bus.Processors.Strategies;

public class BookingRequestedStrategy(IBus bus,
    IRequestClient<IBookingRequested> bookingRequestedEventClient) : IBookingStateProcessorStrategy
{
    private readonly IBus bus = bus;
    private readonly IRequestClient<IBookingRequested> bookingRequestedEventClient = bookingRequestedEventClient;

    public async Task<Response<BookingProcessDto>?> Execute(BookingBaseDto bookingModel)
    {
        var bookingFullModel = bookingModel as BookingDto;

        // Отправка запроса на бронирование
        var bookingRequestedMessage = new BookingRequested
        {
            GuestName = bookingFullModel.GuestName,
            Category = bookingFullModel.Category,
            RoomId = bookingFullModel.RoomId,
            Price = bookingFullModel.Price,            
        };

        if (!bookingModel.IsRequestResponsePattern)
        {
            await bus.Publish<IBookingRequested>(bookingRequestedMessage);
            return null;
        }

        var result = await bookingRequestedEventClient
            .GetResponse<BookingProcessDto>(bookingRequestedMessage);
        return result;
    }
}
