
using MongoDB.Bson;
using MongoDB.Driver;

namespace Bookings.Repositories.Contexts
{
    public interface IMongoDBContext
    {
        IMongoCollection<T> GetCollection<T>(string name);
    }
}
