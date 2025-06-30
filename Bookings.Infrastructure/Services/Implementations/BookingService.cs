using Bookings.Domain;
using Bookings.Domain.Dto;
using Bookings.Domain.Services;
using Bookings.Infrastructure.Documents;
using Bookings.Infrastructure.Mappers;
using Bookings.Domain.Mappers;

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
        var booking = new Booking
        {
            Id = string.IsNullOrEmpty(bookingDto.BookingId) ? Guid.NewGuid().ToString() : bookingDto.BookingId,
            HotelId = bookingDto.HotelId,
            RoomId = bookingDto.RoomId,
            GuestName = bookingDto.GuestName,
            GuestEmail = bookingDto.GuestEmail,
            CheckInDate = DateOnly.FromDateTime(bookingDto.CheckInDate),
            CheckOutDate = DateOnly.FromDateTime(bookingDto.CheckOutDate),
            CreatedAt = bookingDto.CreatedAt == default ? DateTime.UtcNow : bookingDto.CreatedAt,
            UpdatedAt = bookingDto.UpdatedAt == default ? DateTime.UtcNow : bookingDto.UpdatedAt,
            Price = bookingDto.Price,
            Status = bookingDto.Status,
            Adults = bookingDto.Adults,
            Kids = bookingDto.Kids,
            Category = bookingDto.Category,
            StateId = Guid.TryParse(bookingDto.StateId, out var stateId) ? stateId : Guid.NewGuid()
        };

        await bookingRepositoryAdapter.CreateAsync(booking);
        return DtoMapper.BookingToBookingDto(booking);
    }

    public async Task<BookingDto?> GetBookingByIdAsync(string id)
    {
        var booking = await bookingRepositoryAdapter.GetByIdAsync(id);
        return booking != null ? DtoMapper.BookingToBookingDto(booking) : null;
    }

    public async Task<IList<BookingDto>> GetAllBookingsAsync(int page = 0, int count = 30)
    {
        var bookings = await bookingRepositoryAdapter.GetAsync(page, count);
        return bookings.Select(DtoMapper.BookingToBookingDto).ToList();
    }

    public async Task<BookingDto> UpdateBookingAsync(string id, BookingDto bookingDto)
    {
        var existingBooking = await bookingRepositoryAdapter.GetByIdAsync(id) ?? throw new ArgumentException($"Бронирование с ID {id} не найдено");
        var updatedBooking = existingBooking with
        {
            HotelId = bookingDto.HotelId,
            RoomId = bookingDto.RoomId,
            GuestName = bookingDto.GuestName,
            GuestEmail = bookingDto.GuestEmail,
            CheckInDate = DateOnly.FromDateTime(bookingDto.CheckInDate),
            CheckOutDate = DateOnly.FromDateTime(bookingDto.CheckOutDate),
            UpdatedAt = DateTime.UtcNow,
            Price = bookingDto.Price,
            Status = bookingDto.Status,
            Adults = bookingDto.Adults,
            Kids = bookingDto.Kids,
            Category = bookingDto.Category,
            StateId = Guid.TryParse(bookingDto.StateId, out var stateId) ? stateId : existingBooking.StateId
        };

        await bookingRepositoryAdapter.UpdateAsync(updatedBooking);
        return DtoMapper.BookingToBookingDto(updatedBooking);
    }

    public async Task<bool> DeleteBookingAsync(string id)
    {
        var booking = await bookingRepositoryAdapter.GetByIdAsync(id);
        if (booking == null)
        {
            return false;
        }

        await bookingRepositoryAdapter.DeleteAsync(id);
        return true;
    }
} 