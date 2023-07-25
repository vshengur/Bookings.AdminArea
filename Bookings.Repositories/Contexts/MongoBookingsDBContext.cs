using Bookings.Repositories.Models.Settings;

using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

using MongoDB.Driver;
using Bookings.Domain;

namespace Bookings.Repositories.Contexts
{
    /// <summary>
    /// 
    /// </summary>
    public class MongoBookingsDBContext : IMongoDBContext
    {
        /// <summary>
        /// Логгер
        /// </summary>
        private readonly ILogger _logger;

        private IMongoDatabase _db { get; set; }

        private MongoClient _mongoClient { get; set; }

        public IClientSessionHandle Session { get; set; }

        /// <inheritdoc/>
        public readonly IMongoCollection<Hotel> HotelsCollection;

        /// <inheritdoc/>
        public readonly IMongoCollection<Booking> BookingsCollection;

        /// <inheritdoc/>
        public Collation CaseInsensitiveCollation { get; } = new Collation("ru", strength: CollationStrength.Primary);

        public MongoBookingsDBContext(
            ILogger<MongoBookingsDBContext> logger, 
            IOptions<BookingsStoreDatabaseSettings> configuration)
        {
            _logger = logger;

            var connectionString = configuration.Value.ConnectionString;
            _logger.LogInformation("Инициализация соединения с БД \"{connectionString}\" началась.", connectionString);

            _mongoClient = new MongoClient(configuration.Value.ConnectionString);
            _db = _mongoClient.GetDatabase(configuration.Value.DatabaseName);


            HotelsCollection = GetCollection<Hotel>(configuration.Value.HotelsCollectionName);
            BookingsCollection = GetCollection<Booking>(configuration.Value.BookingsCollectionName);

            //InitIndexes();

            _logger.LogInformation("Инициализация соединения с БД \"{connectionString}\" завершилась успешно.", connectionString);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _db.GetCollection<T>(name);
        }


        #region Private

        /// <summary>
        /// Создание индексов БД.
        /// Если запросы обращений по списку будут работать на большой выборке,
        /// возможно нужно будет создать индекс на все возможные сочетания параметров фильтрации.
        /// </summary>
        private void InitIndexes()
        {
            _logger.LogDebug("Start initialize Indexes");

            /// Сначала инициализируем данные индексы,
            /// т.к. по их запуску сбрасываются все остальные индексы
            Task.WaitAll(
                CreateRecourseCollectionIndex(HotelsCollection),
                CreateRecourseCollectionIndex(BookingsCollection)
            );

            _logger.LogDebug("End initialize Indexes");
        }

        /// <summary>
        /// Создаёт индекс для коллекции с обращениями
        /// </summary>
        /// <typeparam name="T">Модель обращения</typeparam>
        /// <param name="collection">Коллекция</param>
        /// <returns>Task</returns>
        private async Task CreateRecourseCollectionIndex<T>(IMongoCollection<T> collection) where T : BaseObject
        {
            var indexBuilder = Builders<T>.IndexKeys;
            IndexKeysDefinition<T>? keys = null;

            keys = indexBuilder.Ascending(x => x.Id);

            var indexModel = new CreateIndexModel<T>(
                keys,
                options: new CreateIndexOptions
                {
                    Collation = CaseInsensitiveCollation,
                }
            );

            collection.Indexes.DropAll();

            await collection.Indexes.CreateOneAsync(indexModel);
        }

        #endregion
    }
}
