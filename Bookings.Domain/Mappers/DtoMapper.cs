using Bookings.Domain.Dto;

namespace Bookings.Domain.Mappers;

public static class DtoMapper
{
    public static BookingDto BookingToBookingDto(Booking booking)
    {
        return new BookingDto
        {
            BookingId = booking.Id.ToString(),
            HotelId = booking.HotelId.ToString(),
            RoomId = booking.RoomId.ToString(),
            GuestName = booking.GuestName,
            GuestEmail = booking.GuestEmail,
            CheckInDate = booking.CheckInDate.ToDateTime(TimeOnly.MinValue),
            CheckOutDate = booking.CheckOutDate.ToDateTime(TimeOnly.MinValue),
            Price = booking.Price,
            Status = booking.Status,
            Adults = booking.Adults,
            Kids = booking.Kids,
            Category = booking.Category,
            StateId = booking.StateId.ToString(),
            CreatedAt = booking.CreatedAt,
            UpdatedAt = booking.UpdatedAt
        };
    }

    public static HotelDto HotelToHotelDto(Hotel hotel)
    {
        return new HotelDto
        {
            Name = hotel.Name,
            City = hotel.City,
            Country = hotel.Country,
            Stars = hotel.Stars,
            Rate = hotel.Rate,
            LocationX = hotel.LocationX,
            LocationY = hotel.LocationY
        };
    }
}
