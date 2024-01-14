namespace Bookings.Domain;

public class Price : BaseObject
{
    public Price() : base() { }

    public Room Room { get; set; } = null!;

    public double Amount { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }
}