using Bookings.Domain.Dto;

namespace Bookings.Infrastructure.Services.Abstractions;

/// <summary>
/// 
/// </summary>
public interface IBookingService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="page"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    Task<IList<BookingDto>> GetBookingsAsync(int page = 0, int count = 30);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="page"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    Task<IList<BookingDto>> GetBookingsAsync(string id);
}
