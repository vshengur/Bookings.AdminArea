using Bookings.Domain.Dto;

namespace Bookings.Domain.Services;

/// <summary>
/// Сервис для получения данных о бронированиях через gRPC
/// </summary>
public interface IBookingQueryService
{
    /// <summary>
    /// Получить список бронирований с пагинацией
    /// </summary>
    /// <param name="page">Номер страницы</param>
    /// <param name="count">Количество элементов на странице</param>
    /// <returns>Список бронирований</returns>
    Task<IList<BookingDto>> GetBookingsAsync(int page = 0, int count = 30);

    /// <summary>
    /// Получить бронирование по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор бронирования</param>
    /// <returns>Список бронирований (обычно один элемент)</returns>
    Task<IList<BookingDto>> GetBookingsAsync(string id);
} 