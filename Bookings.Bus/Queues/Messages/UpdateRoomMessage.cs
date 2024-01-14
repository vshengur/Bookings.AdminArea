namespace Bookings.Bus.Queues.Messages;

public record UpdateRoomMessage
{
    public string HotelId { get; set; }

    public string RoomId { get; set; }

    public string Name { get; set; }

    public int MaxPersons { get; set; }

    public int AdditionalFreeKids { get; set; }
}
