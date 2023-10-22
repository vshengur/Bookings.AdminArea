using Bookings.Bus.Sagas.Events.Abstractions;
using MassTransit;

namespace Bookings.Storage.Queues.Consumers;

public class BookingCancelledConsumer : IConsumer<IBookingCancelled>
{
    public Task Consume(ConsumeContext<IBookingCancelled> context)
    {
        // Ваш код для обработки отмены бронирования
        // Например, может быть вызван метод саги для обработки события BookingCancelled
        return Task.CompletedTask;
    }
}
