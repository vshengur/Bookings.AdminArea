namespace Bookings.Bus.Sagas.Events.Realizations;

public record BaseBookingRequest
{
    public BaseBookingRequest()
    {
        CorrelationId = Guid.NewGuid();
        Timestamp = DateTime.UtcNow;
    }

    public DateTime Timestamp { get; private set; }

    public Guid CorrelationId { get; private set; }
}
