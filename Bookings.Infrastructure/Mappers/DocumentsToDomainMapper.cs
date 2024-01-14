using Bookings.Domain;
using Riok.Mapperly.Abstractions;
using Bookings.Infrastructure.Documents;

namespace Bookings.Infrastructure.Mappers;

[Mapper]
public partial class BookingsMapper : IDocumentMapper<Booking, BookingDocument>
{
    public partial Booking FromDocument(BookingDocument booking);

    public partial BookingDocument ToDocument(Booking booking);
}

[Mapper]
public partial class HotelsMapper : IDocumentMapper<Hotel, HotelDocument>
{
    public partial Hotel FromDocument(HotelDocument booking);

    public partial HotelDocument ToDocument(Hotel booking);
}

[Mapper]
public partial class RoomsMapper : IDocumentMapper<Room, RoomDocument>
{
    public partial Room FromDocument(RoomDocument booking);

    public partial RoomDocument ToDocument(Room booking);
}