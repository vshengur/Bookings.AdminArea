
using Bookings.Domain;
using Bookings.Repositories.Contexts;

using Microsoft.Extensions.Logging;

using MongoDB.Bson;
using MongoDB.Driver;

namespace Bookings.Repositories;

public abstract class BaseRepository<TEntity>
    : IBaseRepository<TEntity> where TEntity : BaseObject
{
    protected readonly IMongoDBContext _mongoContext;
    protected readonly ILogger<BaseRepository<TEntity>> _logger;
    protected IMongoCollection<TEntity> _dbCollection;

    protected BaseRepository(
        IMongoDBContext context,
        ILogger<BaseRepository<TEntity>> logger)
    {
        _mongoContext = context;
        _logger = logger;

        var mongoCollection = typeof(TEntity).Name + "s";
        _logger.LogTrace("Getting collection {mongoCollection}", mongoCollection);

        _dbCollection = _mongoContext.GetCollection<TEntity>(mongoCollection);
    }

    public async Task Create(TEntity obj)
    {
        if (obj == null)
        {
            throw new ArgumentNullException(typeof(TEntity).Name + " object is null");
        }

        await _dbCollection.InsertOneAsync(obj);
    }

    public void Delete(string id)
    {
        var objectId = new ObjectId(id);
        _dbCollection.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", objectId));

    }

    public async Task<TEntity> Get(string id)
    {
        var objectId = new ObjectId(id);

        FilterDefinition<TEntity> filter = Builders<TEntity>.Filter.Eq("_id", objectId);

        return await (await _dbCollection.FindAsync(_ => _.Id == id)).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<TEntity>> Get()
    {
        var all = await _dbCollection.FindAsync(Builders<TEntity>.Filter.Empty);
        return await all.ToListAsync();
    }

    public void Update(TEntity obj)
    {
        _dbCollection.ReplaceOneAsync(Builders<TEntity>.Filter.Eq("_id", obj.Id), obj);
    }
}