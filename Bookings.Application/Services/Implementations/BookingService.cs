namespace Bookings.Infrastructure.Services.Implementations;

using System.Threading.Tasks;

using Bookings.Contracts;
using Bookings.Infrastructure.Services.Abstractions;

/// <summary>
/// Implementation of. <seealso cref="IBookingService"/>
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="BookingService"/> class.
/// </remarks>
/// <param name="grpcBookingClient">gRPC booking client.</param>
public class BookingService(BookingsContract.BookingsContractClient grpcBookingClient) : IBookingService
{
    private readonly BookingsContract.BookingsContractClient grpcBookingClient = grpcBookingClient;

    public async Task<BookingsResponse> GetBookingsAsync(int page = 0, int count = 30)
    {
        var bookings = await grpcBookingClient.GetBookingsAsync(
            new BookingsRequest
            {
                Count = count,
                Page = page,
            },
            deadline: DateTime.UtcNow.AddSeconds(10));

        return bookings;
    }
}
