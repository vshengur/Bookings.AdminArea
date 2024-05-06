using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Bookings.Infrastructure.Documents;

public class BaseDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public string Id { get; private set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public DateTime Created { get; set; }

    public DateTime? EditedAt { get; set; }
}