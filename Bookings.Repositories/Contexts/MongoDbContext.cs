using Bookings.Domain;
using Bookings.Repositories.Models.Settings;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using MongoDB.Driver;

namespace Bookings.Repositories.Contexts;

public class MongoDbContext : IMongoDBContext
{
    /// <inheritdoc/>
    private Collation CaseInsensitiveCollation { get; } = new Collation("ru", strength: CollationStrength.Primary);

    /// <summary>
    /// Логгер
    /// </summary>
    public readonly ILogger _logger;

    private readonly IMongoDatabase _db;

    private readonly MongoClient _mongoClient;

    private IClientSessionHandle Session { get; set; }

    internal MongoDbContext(
        ILogger logger,
        IOptions<BookingsStoreDatabaseSettings> configuration)
    {
        _logger = logger;

        var connectionString = configuration.Value.ConnectionString;
        _logger.LogInformation("Инициализация соединения с БД \"{connectionString}\" началась.", connectionString);

        _mongoClient = new MongoClient(configuration.Value.ConnectionString);
        _db = _mongoClient.GetDatabase(configuration.Value.DatabaseName);

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
    private void InitIndexes(string[] collectionNames)
    {
        _logger.LogDebug("Start initialize Indexes");

        /// Сначала инициализируем данные индексы,
        /// т.к. по их запуску сбрасываются все остальные индексы
        //Task.WaitAll(
        //    collectionNames.Select(collectionName => CreateRecourseCollectionIndex(collectionName))
        //);

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
