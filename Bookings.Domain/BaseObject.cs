using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Bookings.Domain
{
    public class BaseObject
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; private set; }

        public BaseObject()
        {
        }

        public void GenerateId()
        {
            this.Id = ObjectId.GenerateNewId().ToString();
        }
    }
}
