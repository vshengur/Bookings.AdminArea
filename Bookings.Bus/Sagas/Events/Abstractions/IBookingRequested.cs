namespace Bookings.Bus.Sagas.Events.Abstractions;

public interface IBookingRequested : IBookingBaseRequest
{
    Guid StateId { get; set; }

    // Дополнительные свойства для запроса бронирования
    string GuestName { get; set; }

    double Price { get; set; }

    string Category { get; set; }

    string RoomId { get; set; }
}