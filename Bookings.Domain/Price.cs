namespace Bookings.Domain;

/// <summary>
/// Доменная сущность цены
/// </summary>
public record Price : BaseObject
{
    public Room Room { get; init; } = null!;
    public double Amount { get; init; }
    public DateOnly StartDate { get; init; }
    public DateOnly EndDate { get; init; }

    public Price() : base() { }

    public Price(Room room, double amount, DateOnly startDate, DateOnly endDate) : base()
    {
        Room = room;
        Amount = amount;
        StartDate = startDate;
        EndDate = endDate;
    }
}