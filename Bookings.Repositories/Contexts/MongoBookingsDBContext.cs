using Bookings.Repositories.Models.Settings;

using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

using MongoDB.Driver;
using Bookings.Domain;

namespace Bookings.Repositories.Contexts;

/// <summary>
/// 
/// </summary>
public class MongoBookingsDBContext : MongoDbContext
{
    /// <inheritdoc/>
    public readonly IMongoCollection<Booking> BookingsCollection;

    public MongoBookingsDBContext(
        ILogger<MongoBookingsDBContext> logger,
        IOptions<BookingsStoreDatabaseSettings> configuration)
        :base (logger, configuration)
    {
        _logger.LogInformation(@"Начало создания таблиц для контекста.");

        BookingsCollection = GetCollection<Booking>(configuration.Value.BookingsCollectionName);

        //InitIndexes();

        _logger.LogInformation(@"Окончание создания таблиц для контекста.");
    }
}

/// <summary>
/// 
/// </summary>
public class MongoHotelsDBContext : MongoDbContext
{
    /// <inheritdoc/>
    public readonly IMongoCollection<Hotel> HotelsCollection;

    /// <inheritdoc/>
    public readonly IMongoCollection<Room> RoomsCollection;

    public MongoHotelsDBContext(
        ILogger<MongoBookingsDBContext> logger,
        IOptions<BookingsStoreDatabaseSettings> configuration)
        : base(logger, configuration)
    {
        _logger.LogInformation(@"Начало создания таблиц для контекста.");

        HotelsCollection = GetCollection<Hotel>(configuration.Value.HotelsCollectionName);
        RoomsCollection = GetCollection<Room>(configuration.Value.RoomsCollectionName);

        //InitIndexes();

        _logger.LogInformation(@"Окончание создания таблиц для контекста.");
    }
}

