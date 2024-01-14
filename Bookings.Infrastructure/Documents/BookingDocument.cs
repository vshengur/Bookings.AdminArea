using MongoDB.Bson.Serialization.Attributes;

namespace Bookings.Infrastructure.Documents;

public class BookingDocument : BaseDocument
{
    [BsonElement("Name")]
    public string BookName { get; set; } = null!;

    public RoomDocument Room { get; set; } = null!;

    public double Price { get; set; }

    public string Category { get; set; } = null!;

    public Guid StateId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public int Adults { get; set; }

    public int Kids { get; set; }
}
