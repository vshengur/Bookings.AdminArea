using Bookings.Contracts;
using Bookings.Domain;
using Bookings.Domain.Repositories;

using Grpc.Core;

namespace Bookings.Api.Endpoints;

public class BookingsEndpoint(
    IBookingsRepository bookingsRepository)
    : BookingsContract.BookingsContractBase
{
    /// <inheritdoc/>
    public override async Task<BookingsResponse?> GetBookings(BookingsRequest request, ServerCallContext context)
    {
        IEnumerable<Booking> bookings;

        if (!string.IsNullOrWhiteSpace(request.Id))
        {
            bookings = [await bookingsRepository.Get(request.Id)];
        }
        else
        {
            bookings = await bookingsRepository.Get(request.Page, request.Count);
        }

        if (bookings == null)
        {
            return null;
        }

        BookingsResponse response = new();
        response.Bookings.AddRange(bookings.Select(_ =>
            new BookingItem
            {
                Id = _.Id.ToString(),
                BookName = _.BookName,
                Category = _.Category,

                // TODO: replace with room
                // HotelId = _.Hotel.Id,
                Price = _.Price
            }));

        return response;
    }
}