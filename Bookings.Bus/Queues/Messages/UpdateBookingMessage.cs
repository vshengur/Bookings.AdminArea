namespace Bookings.Bus.Queues.Messages;

public record UpdateBookingMessage
{
    public string BookingId { get; init; }
    public string HotelId { get; init; }
    public string RoomId { get; init; }
    public string GuestName { get; init; }
    public string GuestEmail { get; init; }
    public DateTime CheckInDate { get; init; }
    public DateTime CheckOutDate { get; init; }
    public double Price { get; init; }
    public string Status { get; init; }
    public int Adults { get; init; }
    public int Kids { get; init; }
    public string Category { get; init; }
    public string StateId { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}
