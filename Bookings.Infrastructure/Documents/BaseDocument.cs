using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Bookings.Infrastructure.Documents;

public class BaseDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; private set; }

    public DateTime Created { get; set; }

    public DateTime? EditedAt { get; set; }
}