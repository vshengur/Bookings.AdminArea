using Bookings.Domain.DTO;
using Bookings.Domain.DTO.BookingProcess;

using MassTransit;

namespace Bookings.Services.Interfaces
{
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
        Task<Response<BookingProcessDto>?> ProcessRequest(BookingDTO bookingDTO);
    }
}
