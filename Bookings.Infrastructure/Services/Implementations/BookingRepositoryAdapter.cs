using Bookings.Domain;
using Bookings.Domain.Repositories;
using Bookings.Infrastructure.Documents;
using Bookings.Infrastructure.Mappers;

namespace Bookings.Infrastructure.Services.Implementations;

/// <summary>
/// Адаптер для работы с репозиторием бронирований
/// </summary>
/// <remarks>
/// Инициализирует новый экземпляр класса <see cref="BookingRepositoryAdapter"/>.
/// </remarks>
/// <param name="bookingsRepository">Репозиторий бронирований.</param>
/// <param name="bookingMapper">Маппер для преобразования между доменными объектами и документами.</param>
public class BookingRepositoryAdapter(
    IBookingsRepository bookingsRepository,
    IDocumentMapper<Booking, BookingDocument> bookingMapper)
{
    private readonly IBookingsRepository bookingsRepository = bookingsRepository;
    private readonly IDocumentMapper<Booking, BookingDocument> bookingMapper = bookingMapper;

    /// <summary>
    /// Создать бронирование
    /// </summary>
    /// <param name="booking">Бронирование для создания</param>
    public async Task CreateAsync(Booking booking)
    {
        await bookingsRepository.Create(booking);
    }

    /// <summary>
    /// Получить бронирование по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор бронирования</param>
    /// <returns>Бронирование</returns>
    public async Task<Booking?> GetByIdAsync(string id)
    {
        return await bookingsRepository.Get(id);
    }

    /// <summary>
    /// Получить список бронирований с пагинацией
    /// </summary>
    /// <param name="page">Номер страницы</param>
    /// <param name="count">Количество элементов на странице</param>
    /// <returns>Список бронирований</returns>
    public async Task<List<Booking>> GetAsync(int page, int count)
    {
        return await bookingsRepository.Get(page, count);
    }

    /// <summary>
    /// Обновить бронирование
    /// </summary>
    /// <param name="booking">Бронирование для обновления</param>
    public async Task UpdateAsync(Booking booking)
    {
        await bookingsRepository.Update(booking);
    }

    /// <summary>
    /// Удалить бронирование по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор бронирования</param>
    public async Task DeleteAsync(string id)
    {
        await bookingsRepository.Delete(id);
    }
} 