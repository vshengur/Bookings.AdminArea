using Bookings.Domain.Dto;

namespace Bookings.Domain.Services;

/// <summary>
/// Сервис для управления бронированиями (CRUD операции)
/// </summary>
public interface IBookingService
{
    /// <summary>
    /// Создать новое бронирование
    /// </summary>
    /// <param name="bookingDto">Данные бронирования</param>
    /// <returns>Созданное бронирование</returns>
    Task<BookingDto> CreateBookingAsync(BookingDto bookingDto);

    /// <summary>
    /// Получить бронирование по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор бронирования</param>
    /// <returns>Бронирование</returns>
    Task<BookingDto?> GetBookingByIdAsync(string id);

    /// <summary>
    /// Получить список всех бронирований с пагинацией
    /// </summary>
    /// <param name="page">Номер страницы</param>
    /// <param name="count">Количество элементов на странице</param>
    /// <returns>Список бронирований</returns>
    Task<IList<BookingDto>> GetAllBookingsAsync(int page = 0, int count = 30);

    /// <summary>
    /// Обновить бронирование
    /// </summary>
    /// <param name="id">Идентификатор бронирования</param>
    /// <param name="bookingDto">Новые данные бронирования</param>
    /// <returns>Обновленное бронирование</returns>
    Task<BookingDto> UpdateBookingAsync(string id, BookingDto bookingDto);

    /// <summary>
    /// Удалить бронирование
    /// </summary>
    /// <param name="id">Идентификатор бронирования</param>
    /// <returns>True если удаление прошло успешно</returns>
    Task<bool> DeleteBookingAsync(string id);
} 