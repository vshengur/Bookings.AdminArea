using MassTransit;

namespace Bookings.Bus.Sagas.States;

public class BookingMainSagaState : SagaStateMachineInstance, ISagaVersion
{
    public Guid CorrelationId { get; set; }
    public int Version { get; set; }
    public int CurrentState { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Confirmed { get; set; }
    public DateTime? Cancelled { get; set; }
}
