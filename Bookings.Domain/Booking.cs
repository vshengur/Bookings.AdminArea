using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Bookings.Domain;

public class Booking: BaseObject
{
    [BsonElement("Name")]
    public string BookName { get; set; } = null!;

    public Hotel Hotel { get; set; } = null!;

    public double Price { get; set; }

    public string Category { get; set; } = null!;

    public Guid StateId { get; set; }

    public Booking()
        :base()
    {
    }
}