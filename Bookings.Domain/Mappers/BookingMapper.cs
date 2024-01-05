using Bookings.Domain.Dto;

using Riok.Mapperly.Abstractions;

namespace Bookings.Domain.Mappers
{
    [Mapper]
    public partial class BookingMapper
    {
        public partial BookingDto BookingToBookingDto(Booking booking);
    }
}
