namespace Bookings.Bus.Queues.Messages;

public record CreateRoomMessage
{
    public string Name { get; set; } = null!;

    public int MaxPersons { get; set; }

    public int AdditionalFreeKids { get; set; }

    public string HotelId { get; set; }

}
