using Bookings.Bus.Sagas.Events.Abstractions;
using Bookings.Bus.Sagas.Events.Realizations;
using Bookings.Domain.Dto;
using Bookings.Domain.Dto.BookingProcess;

using MassTransit;

namespace Bookings.Bus.Processors.Strategies;

public class BookingCancelledStrategy : IBookingStateProcessorStrategy
{
    private readonly IBus bus;
    private readonly IRequestClient<IBookingCancelled> bookingCancelledEventClient;

    public BookingCancelledStrategy(
        IBus bus,
        IRequestClient<IBookingCancelled> bookingCancelledEventClient)
    {
        this.bus = bus;
        this.bookingCancelledEventClient = bookingCancelledEventClient;
    }

    public async Task<Response<BookingProcessDto>?> Execute(BookingBaseDto bookingModel)
    {
        // Отправка запроса на бронирование
        var bookingCancelledMessage = new BookingCancelled
        {
            // Дополнительные свойства для запроса бронирования
            BookingId = bookingModel.BookingId,
        };

        if (!bookingModel.IsRequestResponsePattern)
        {
            await bus.Publish<IBookingCancelled>(bookingCancelledMessage);
            return null;
        }

        var result = await bookingCancelledEventClient
            .GetResponse<BookingProcessDto>(bookingCancelledMessage);

        return result;
    }
}
