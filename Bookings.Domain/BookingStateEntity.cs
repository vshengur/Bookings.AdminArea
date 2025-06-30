namespace Bookings.Domain;

public record BookingStateEntity : BaseObject
{
    public string BookingId { get; set; } = default!;
    public BookingStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}