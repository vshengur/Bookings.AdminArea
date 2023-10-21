using Bookings.Bus.Sagas.Events.Abstractions;
using Bookings.Bus.Sagas.States;
using MassTransit;

namespace Bookings.Bus.Sagas.StateMachine;

public class BookingMainSaga : MassTransitStateMachine<BookingMainSagaState>
{
    public BookingMainSaga()
    {
        InstanceState(x => x.CurrentState);

        Event(() => BookingRequested, x => x.CorrelateById(m => m.Message.CorrelationId));
        Event(() => BookingConfirmed, x => x.CorrelateById(m => m.Message.CorrelationId));
        Event(() => BookingCancelled, x => x.CorrelateById(m => m.Message.CorrelationId));

        Initially(
            When(BookingRequested)
                .Then(context => {
                    context.Saga.Created = context.Message.Timestamp;
                    return;
                })
                .TransitionTo(Pending));

        During(Pending,
            When(BookingConfirmed)
                .Then(context =>
                {
                    context.Saga.Confirmed = context.Message.Timestamp;
                    return;
                })
                .TransitionTo(Confirmed),
            When(BookingCancelled)
                .Then(context =>
                {
                    context.Saga.Cancelled = context.Message.Timestamp;
                    return;
                })
                .TransitionTo(Cancelled));

        During(Confirmed,
            When(BookingCancelled)
                .Then(context => {
                    context.Saga.Cancelled = context.Message.Timestamp;
                    return;
                })
                .TransitionTo(Cancelled));

        SetCompletedWhenFinalized();
    }

    public State Pending { get; private set; }
    public State Confirmed { get; private set; }
    public State Cancelled { get; private set; }

    public Event<IBookingRequested> BookingRequested { get; private set; }
    public Event<IBookingConfirmed> BookingConfirmed { get; private set; }
    public Event<IBookingCancelled> BookingCancelled { get; private set; }
}
