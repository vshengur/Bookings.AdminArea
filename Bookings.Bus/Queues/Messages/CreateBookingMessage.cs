namespace Bookings.Bus.Queues.Messages;

public record CreateBookingMessage
{
    public string BookName { get; init; }

    public DateTime CreatedDate { get; init; }

    public double Price { get; init; }

    public string Category { get; init; }

    public string HotelId { get; init; }
}
