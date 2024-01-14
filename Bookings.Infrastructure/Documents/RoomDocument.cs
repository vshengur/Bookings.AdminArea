using MongoDB.Bson.Serialization.Attributes;

namespace Bookings.Infrastructure.Documents;

public class RoomDocument : BaseDocument
{
    public RoomDocument() : base() { }

    [BsonElement("Name")]
    public string Name { get; set; } = null!;

    public int MaxPersons { get; set; }

    public int AdditionalFreeKids { get; set; }

    public HotelDocument Hotel { get; set; } = null!;
}
