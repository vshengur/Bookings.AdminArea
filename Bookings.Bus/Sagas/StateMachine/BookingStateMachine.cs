using Bookings.Bus.Sagas.States;
using MassTransit;

namespace Bookings.Bus.Sagas.StateMachine
{
    public class BookingStateMachine : MassTransitStateMachine<BookingState>
    {
        public State Submitted { get; private set; }

        public State Accepted { get; private set; }

        public State Canceled { get; private set; }

        public BookingStateMachine()
        {
            InstanceState(x => x.CurrentState, Submitted, Accepted, Canceled);

            Initially(
                When(SubmitBookingEvent)
                    .Then(x => x.Saga.BookingDate = x.Message.BookingDate)
                    .TransitionTo(Submitted),
                When(BookingAcceptedEvent)
                    .TransitionTo(Accepted));

            During(Submitted, When(BookingAcceptedEvent)
                    .TransitionTo(Accepted));

            During(Accepted, When(SubmitBookingEvent)
                    .Then(x => x.Saga.BookingDate = x.Message.BookingDate));

            DuringAny(
                When(BookingCancellationRequested)
                    .RespondAsync(context => context.Init<BookingCanceled>(new { OrderId = context.Saga.CorrelationId }))
                    .TransitionTo(Canceled));

            During(Accepted,
                Ignore(SubmitBookingEvent));


            Event(() => SubmitBookingEvent,
                x => x.CorrelateById(context => context.Message.BookingId));

            Event(() => BookingAcceptedEvent, 
                x => x.CorrelateById(context => context.Message.BookingId));

            // Event(() => BookingCanceled); // not required, as it is the default convention

            Event(() => BookingCancellationRequested, e =>
            {
                e.CorrelateById(context => context.Message.BookingId);

                e.OnMissingInstance(m =>
                {
                    return m.ExecuteAsync(x => x.RespondAsync<BookingNotFound>(new { x.Message.BookingId }));
                });
            });
        }

        public Event<BookingAccepted> BookingAcceptedEvent { get; private set; }

        public Event<SubmitBooking> SubmitBookingEvent { get; private set; }

        public Event<RequestBookingCancellation> BookingCancellationRequested { get; private set; }

    }

    public interface SubmitBooking
    {
        Guid BookingId { get; }

        DateTime BookingDate { get; }
    }

    public interface BookingAccepted
    {
        Guid BookingId { get; }
    }

    public interface BookingCanceled : CorrelatedBy<Guid>
    {
    }

    public interface RequestBookingCancellation
    {
        Guid BookingId { get; }
    }

    public interface BookingNotFound
    {
        Guid BookingId { get; }
    }
}
