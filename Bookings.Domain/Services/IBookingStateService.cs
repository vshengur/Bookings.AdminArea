using Bookings.Domain.Dto;
using Bookings.Domain.Dto.BookingProcess;

using MassTransit;

namespace Bookings.Domain.Services;

/// <summary>
/// Сервис управления состоянием бронирования
/// </summary>
public interface IBookingStateService
{
    /// <summary>
    /// Обработать запрос бронирования
    /// </summary>
    /// <param name="bookingDTO">Модель бронирования</param>
    /// <returns>Результат обработки</returns>
    Task<Response<BookingProcessDto>> ProcessRequest(BookingBaseDto bookingDTO);
} 