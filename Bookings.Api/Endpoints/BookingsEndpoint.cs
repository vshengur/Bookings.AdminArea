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

        BookingsResponse response = new ();
        response.Bookings.AddRange(bookings.Select(_ =>
            new BookingItem
            {
                Id = _.Id.ToString(),
                HotelId = _.HotelId,
                GuestName = _.GuestName,
                Category = _.Category,
                Price = _.Price,
                RoomId = _.RoomId,
                GuestEmail = _.GuestEmail,
                CheckInDate = _.CheckInDate.ToString("O"),
                CheckOutDate = _.CheckOutDate.ToString("O"),
                CreatedAt = _.CreatedAt.ToString("O"),
                UpdatedAt = _.UpdatedAt.ToString("O"),
                Status = _.Status,
                Adults = _.Adults,
                Kids = _.Kids,
                StateId = _.StateId.ToString()
            }));

        return response;
    }
}