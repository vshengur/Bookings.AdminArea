namespace Bookings.Domain.DTO;

public class HotelDTO
{
    public string Name { get; set; }

    public string City { get; set; }

    public int Stars { get; set; }

    public int RoomsCount { get; set; }

    public double LocationX { get; set; }

    public double LocationY { get; set; }
}
