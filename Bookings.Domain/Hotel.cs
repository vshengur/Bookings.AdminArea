namespace Bookings.Domain;

public class Hotel : BaseObject
{
    public Hotel() : base() { }

    public Hotel(string id, DateTime created) : base(id, created) { }

    public string Name { get; set; }

    public string City { get; set; }

    public string Country { get; set; }

    public int Stars { get; set; }

    public int Rate { get; set; }

    public double LocationX { get; set; }

    public double LocationY { get; set; }
}