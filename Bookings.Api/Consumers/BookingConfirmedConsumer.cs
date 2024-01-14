using Bookings.Bus.Sagas.Events.Abstractions;
using MassTransit;

namespace Bookings.Api.Consumers;

public class BookingConfirmedConsumer : IConsumer<IBookingConfirmed>
{
    public Task Consume(ConsumeContext<IBookingConfirmed> context)
    {
        // Ваш код для обработки подтверждения бронирования
        // Например, может быть вызван метод саги для обработки события BookingConfirmed
        return Task.CompletedTask;
    }
}
