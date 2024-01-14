namespace Bookings.Bus.Queues.Messages;

public class UpdateHotelMessage
{
    public string HotelId { get; set; }

    public string Name { get; set; }

    public int Stars { get; set; }

    public int Rate { get; set; }
}
