namespace Bookings.Bus.Sagas.Events.Abstractions;

public interface IBookingBaseRequest
{
    DateTime Timestamp { get; }

    public Guid CorrelationId { get; }
}
