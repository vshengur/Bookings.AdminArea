using Bookings.Contracts;
using Bookings.Domain.Dto;
using Bookings.Domain.Services;

namespace Bookings.Infrastructure.Services.Implementations;

/// <summary>
/// Реализация сервиса для получения данных о бронированиях через gRPC
/// </summary>
/// <remarks>
/// Инициализирует новый экземпляр класса <see cref="BookingQueryService"/>.
/// </remarks>
/// <param name="grpcBookingClient">gRPC клиент для работы с бронированиями.</param>
public class BookingQueryService(BookingsContract.BookingsContractClient grpcBookingClient) : IBookingQueryService
{
    private readonly BookingsContract.BookingsContractClient grpcBookingClient = grpcBookingClient;

    public async Task<IList<BookingDto>> GetBookingsAsync(int page = 0, int count = 30)
    {
        var bookings = await grpcBookingClient.GetBookingsAsync(
            new BookingsRequest
            {
                Count = count,
                Page = page,
            },
            deadline: DateTime.UtcNow.AddSeconds(10));

        return bookings.Bookings
            .Select(_ => new BookingDto() { })
            .ToList();
    }

    public async Task<IList<BookingDto>> GetBookingsAsync(string id)
    {
        var bookings = await grpcBookingClient.GetBookingsAsync(
            new BookingsRequest
            {
                Id = id
            },
            deadline: DateTime.UtcNow.AddSeconds(10));

        return bookings.Bookings
            .Select(_ => new BookingDto() { })
            .ToList();
    }
} 