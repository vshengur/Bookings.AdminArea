namespace Bookings.Domain;

public class Room : BaseObject
{
    public Room() : base() { }

    public string Name { get; set; } = null!;

    public int MaxPersons { get; set; }

    public int AdditionalFreeKids { get; set; }

    public Hotel Hotel { get; set; } = null!;
}
