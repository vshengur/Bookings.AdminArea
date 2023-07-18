using Bookings.Repositories.Models.Settings;

using Microsoft.Extensions.Options;

using MongoDB.Driver;

namespace Bookings.Repositories.Contexts
{

    /// <summary>
    /// 
    /// </summary>
    public class MongoBookingsDBContext : IMongoDBContext
    {
        private IMongoDatabase _db { get; set; }

        private MongoClient _mongoClient { get; set; }

        public IClientSessionHandle Session { get; set; }

        public MongoBookingsDBContext(IOptions<BookingsStoreDatabaseSettings> configuration)
        {
            _mongoClient = new MongoClient(configuration.Value.ConnectionString);
            _db = _mongoClient.GetDatabase(configuration.Value.DatabaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _db.GetCollection<T>(name);
        }
    }
}
