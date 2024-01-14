using Bookings.Domain.Dto;
using Bookings.Domain.Dto.BookingProcess;

using MassTransit;

namespace Bookings.Infrastructure.Services.Abstractions;

/// <summary>
/// Booking state service.
/// </summary>
public interface IBookingStateService
{
    /// <summary>
    /// Process booking request.
    /// </summary>
    /// <param name="bookingDTO">Booking model.</param>
    /// <returns></returns>
    Task<Response<BookingProcessDto>> ProcessRequest(BookingBaseDto bookingDTO);
}
