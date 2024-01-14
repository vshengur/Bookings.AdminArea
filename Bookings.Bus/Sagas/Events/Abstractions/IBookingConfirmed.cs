namespace Bookings.Bus.Sagas.Events.Abstractions;

public interface IBookingConfirmed: IBookingBaseRequest
{
    string BookingId { get; }
    // Дополнительные свойства для подтверждения бронирования
}
