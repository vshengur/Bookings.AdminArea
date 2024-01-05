﻿using Bookings.Contracts;

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
    Task<BookingsResponse> GetBookingsAsync(int page = 0, int count = 30);
}
