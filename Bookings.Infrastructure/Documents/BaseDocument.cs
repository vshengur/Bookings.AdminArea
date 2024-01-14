using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Bookings.Infrastructure.Documents;

public abstract class BaseDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; private set; }

    public DateTime Created { get; private set; }

    public DateTime? EditedAt { get; private set; }
}