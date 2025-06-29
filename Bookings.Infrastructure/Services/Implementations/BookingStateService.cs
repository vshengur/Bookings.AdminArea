using Bookings.Bus.Processors;
using Bookings.Bus.Processors.Strategies;
using Bookings.Bus.Sagas.Events.Abstractions;
using Bookings.Domain.Dto;
using Bookings.Domain.Dto.BookingProcess;
using Bookings.Domain.Services;

using MassTransit;

namespace Bookings.Infrastructure.Services.Implementations;

/// <summary>
/// Реализация <seealso cref="IBookingStateService"/>
/// </summary>
/// <remarks>
/// Ctor.
/// </remarks>
/// <param name="bus">Bus abstraction.</param>
/// <param name="bookingRequestedEventClient">Client for event <see cref="IBookingRequested"/>.</param>
/// <param name="bookingCancelledEventClient">Client for event <see cref="IBookingCancelled"/>.</param>
/// <param name="bookingConfirmedEventClient">Client for event <see cref="IBookingConfirmed"/>.</param>
public class BookingStateService(
    IBus bus,
    IRequestClient<IBookingRequested> bookingRequestedEventClient,
    IRequestClient<IBookingCancelled> bookingCancelledEventClient,
    IRequestClient<IBookingConfirmed> bookingConfirmedEventClient) : IBookingStateService
{
    public Task<Response<BookingProcessDto>> ProcessRequest(BookingBaseDto bookingDTO)
    {
        IBookingStateProcessorStrategy bookingStateProcessorStrategy = bookingDTO.State switch
        {
            BookingState.Confirmed => new BookingConfirmedStrategy(bus, bookingConfirmedEventClient),
            BookingState.Cancelled => new BookingCancelledStrategy(bus, bookingCancelledEventClient),
            _ => new BookingRequestedStrategy(bus, bookingRequestedEventClient)
        };

        BookingStateProcessor bookingStateProcessor = new(bookingStateProcessorStrategy);
        return bookingStateProcessor.Proceed(bookingDTO);
    }
}
