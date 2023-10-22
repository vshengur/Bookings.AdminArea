using MassTransit;

namespace Bookings.Bus.Sagas.States;

public class BookingState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }

    public int CurrentState { get; set; }

    public DateTime? BookingDate { get; set; }
}
