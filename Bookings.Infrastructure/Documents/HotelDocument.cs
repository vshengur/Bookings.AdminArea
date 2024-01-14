using MongoDB.Bson.Serialization.Attributes;

namespace Bookings.Infrastructure.Documents;

public class HotelDocument : BaseDocument
{
    public HotelDocument() : base() { }

    [BsonElement("Name")]
    public string Name { get; set; }

    public string City { get; set; }

    public string Country { get; set; }

    public int Stars { get; set; }

    public int Rate { get; set; }

    public double LocationX { get; set; }

    public double LocationY { get; set; }
}
