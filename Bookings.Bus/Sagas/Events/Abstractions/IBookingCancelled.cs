namespace Bookings.Bus.Sagas.Events.Abstractions;

public interface IBookingCancelled : IBookingBaseRequest
{
    string BookingId { get; }
    DateTime Timestamp { get; }
    // Дополнительные свойства для отмены бронирования
}
