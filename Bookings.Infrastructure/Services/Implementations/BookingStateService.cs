﻿using Bookings.Bus.Processors;
using Bookings.Bus.Processors.Strategies;
using Bookings.Bus.Sagas.Events.Abstractions;
using Bookings.Domain.Dto;
using Bookings.Domain.Dto.BookingProcess;
using Bookings.Infrastructure.Services.Abstractions;

using MassTransit;

namespace Bookings.Infrastructure.Services.Implementations;

/// <summary>
/// Realization of <seealso cref="IBookingStateService"/>
/// </summary>
public class BookingStateService : IBookingStateService
{
    private readonly IBus bus;
    private readonly IRequestClient<IBookingRequested> bookingRequestedEventClient;
    private readonly IRequestClient<IBookingCancelled> bookingCancelledEventClient;
    private readonly IRequestClient<IBookingConfirmed> bookingConfirmedEventClient;

    /// <summary>
    /// Ctor.
    /// </summary>
    /// <param name="bus">Bus abstraction.</param>
    /// <param name="bookingRequestedEventClient">Client for event <see cref="IBookingRequested"/>.</param>
    /// <param name="bookingCancelledEventClient">Client for event <see cref="IBookingCancelled"/>.</param>
    /// <param name="bookingConfirmedEventClient">Client for event <see cref="IBookingConfirmed"/>.</param>
    public BookingStateService(
        IBus bus,
        IRequestClient<IBookingRequested> bookingRequestedEventClient,
        IRequestClient<IBookingCancelled> bookingCancelledEventClient,
        IRequestClient<IBookingConfirmed> bookingConfirmedEventClient)
    {
        this.bus = bus;
        this.bookingRequestedEventClient = bookingRequestedEventClient;
        this.bookingCancelledEventClient = bookingCancelledEventClient;
        this.bookingConfirmedEventClient = bookingConfirmedEventClient;
    }

    public async Task<Response<BookingProcessDto>> ProcessRequest(BookingBaseDto bookingDTO)
    {
        IBookingStateProcessorStrategy bookingStateProcessorStrategy = bookingDTO.State switch
        {
            BookingState.Confirmed => new BookingConfirmedStrategy(bus, bookingConfirmedEventClient),
            BookingState.Cancelled => new BookingCancelledStrategy(bus, bookingCancelledEventClient),
            _ => new BookingRequestedStrategy(bus, bookingRequestedEventClient)
        };

        BookingStateProcessor bookingStateProcessor = new(bookingStateProcessorStrategy);
        return await bookingStateProcessor.Proceed(bookingDTO);
    }
}
