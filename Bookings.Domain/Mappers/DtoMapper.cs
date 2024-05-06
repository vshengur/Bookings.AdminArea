using Bookings.Domain.Dto;

using Riok.Mapperly.Abstractions;

namespace Bookings.Domain.Mappers;

[Mapper]
public partial class DtoMapper
{
    public partial BookingDto BookingToBookingDto(Booking booking);
    public partial HotelDto HotelToHotelDto(Hotel booking);
    public partial RoomDto RoomToRoomDto(Room booking);
}
