using Bookings.Domain;

namespace Bookings.Domain.Repositories;

/// <summary>
/// Интерфейс репозитория для работы с состояниями бронирований
/// </summary>
public interface IBookingStateRepository : IBaseRepository<BookingStateEntity>
{
    /// <summary>
    /// Получить состояние бронирования по ID бронирования
    /// </summary>
    Task<BookingStateEntity?> GetByBookingId(string bookingId);

    /// <summary>
    /// Получить все состояния по типу состояния
    /// </summary>
    Task<List<BookingStateEntity>> GetByState(BookingStatus state);
} 