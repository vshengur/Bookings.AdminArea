using Bookings.Domain;
using Bookings.Domain.Dto;
using Bookings.Domain.Dto.BookingProcess;
using Bookings.Domain.Services;
using Bookings.Infrastructure.Documents;
using Bookings.Infrastructure.Mappers;

namespace Bookings.Infrastructure.Services.Implementations;

/// <summary>
/// Реализация сервиса для управления бронированиями (CRUD операции)
/// </summary>
/// <remarks>
/// Инициализирует новый экземпляр класса <see cref="BookingService"/>.
/// </remarks>
/// <param name="bookingRepositoryAdapter">Адаптер репозитория бронирований.</param>
/// <param name="bookingMapper">Маппер для преобразования между доменными объектами и DTO.</param>
public class BookingService(
    BookingRepositoryAdapter bookingRepositoryAdapter,
    IDocumentMapper<Booking, BookingDocument> bookingMapper) : IBookingService
{
    private readonly BookingRepositoryAdapter bookingRepositoryAdapter = bookingRepositoryAdapter;
    private readonly IDocumentMapper<Booking, BookingDocument> bookingMapper = bookingMapper;

    public async Task<BookingDto> CreateBookingAsync(BookingDto bookingDto)
    {
        // Создаем доменный объект из DTO
        var booking = new Booking(
            bookName: bookingDto.BookName ?? string.Empty,
            room: null!, // TODO: Получить Room из репозитория по bookingDto.RoomId
            price: bookingDto.Price,
            category: bookingDto.Category ?? string.Empty,
            stateId: Guid.NewGuid(), // TODO: Использовать правильный StateId
            startDate: DateOnly.FromDateTime(DateTime.Today), // TODO: Добавить в DTO
            endDate: DateOnly.FromDateTime(DateTime.Today.AddDays(1)), // TODO: Добавить в DTO
            adults: 1, // TODO: Добавить в DTO
            kids: 0 // TODO: Добавить в DTO
        );

        await bookingRepositoryAdapter.CreateAsync(booking);

        // Возвращаем созданное бронирование
        return MapToDto(booking);
    }

    public async Task<BookingDto?> GetBookingByIdAsync(string id)
    {
        var booking = await bookingRepositoryAdapter.GetByIdAsync(id);
        return booking != null ? MapToDto(booking) : null;
    }

    public async Task<IList<BookingDto>> GetAllBookingsAsync(int page = 0, int count = 30)
    {
        var bookings = await bookingRepositoryAdapter.GetAsync(page, count);
        return bookings.Select(MapToDto).ToList();
    }

    public async Task<BookingDto> UpdateBookingAsync(string id, BookingDto bookingDto)
    {
        var existingBooking = await bookingRepositoryAdapter.GetByIdAsync(id);
        if (existingBooking == null)
        {
            throw new ArgumentException($"Бронирование с ID {id} не найдено");
        }

        // Обновляем свойства существующего бронирования
        var updatedBooking = existingBooking with
        {
            BookName = bookingDto.BookName ?? existingBooking.BookName,
            Price = bookingDto.Price,
            Category = bookingDto.Category ?? existingBooking.Category
            // TODO: Добавить обновление других свойств
        };

        bookingRepositoryAdapter.UpdateAsync(updatedBooking);

        return MapToDto(updatedBooking);
    }

    public async Task<bool> DeleteBookingAsync(string id)
    {
        var booking = await bookingRepositoryAdapter.GetByIdAsync(id);
        if (booking == null)
        {
            return false;
        }

        bookingRepositoryAdapter.DeleteAsync(id);
        return true;
    }

    private static BookingDto MapToDto(Booking booking)
    {
        return new BookingDto
        {
            BookingId = booking.Id,
            RoomId = booking.Room.Id,
            BookName = booking.BookName,
            Price = booking.Price,
            Category = booking.Category,
            CreatedDate = booking.Created,
            State = BookingState.Created // TODO: Маппить правильное состояние
        };
    }
} 