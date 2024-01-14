using Bookings.Bus.Sagas.Events.Abstractions;
using Bookings.Bus.Sagas.Events.Realizations;
using Bookings.Domain.Dto;
using Bookings.Domain.Dto.BookingProcess;

using MassTransit;

namespace Bookings.Bus.Processors.Strategies;

public class BookingConfirmedStrategy : IBookingStateProcessorStrategy
{
    private readonly IBus bus;
    private readonly IRequestClient<IBookingConfirmed> bookingConfirmedEventClient;

    public BookingConfirmedStrategy(
        IBus bus,
        IRequestClient<IBookingConfirmed> bookingConfirmedEventClient)
    {
        this.bus = bus;
        this.bookingConfirmedEventClient = bookingConfirmedEventClient;
    }

    public async Task<Response<BookingProcessDto>?> Execute(BookingBaseDto bookingModel)
    {
        // Отправка запроса на бронирование
        var bookingConfirmedMessage = new BookingConfirmed
        {
            // Дополнительные свойства для запроса бронирования
            BookingId = bookingModel.BookingId,
        };

        if (!bookingModel.IsRequestResponsePattern)
        {
            await bus.Publish<IBookingConfirmed>(bookingConfirmedMessage);
            return null;
        }

        var result = await bookingConfirmedEventClient
            .GetResponse<BookingProcessDto>(bookingConfirmedMessage);
        return result;
    }
}