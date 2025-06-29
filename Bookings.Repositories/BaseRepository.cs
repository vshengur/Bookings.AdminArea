using Bookings.Domain;
using Bookings.Infrastructure.Documents;
using Bookings.Infrastructure.Mappers;
using Bookings.Repositories.Contexts;

using Microsoft.Extensions.Logging;

using MongoDB.Bson;
using MongoDB.Driver;

namespace Bookings.Repositories;

public abstract class BaseRepository<TDomain, TDocument> : IBaseRepository<TDomain> 
    where TDomain : BaseObject
    where TDocument : BaseDocument
{
    protected readonly IMongoDBContext _mongoContext;
    protected readonly ILogger<BaseRepository<TDomain, TDocument>> _logger;
    protected readonly IMongoCollection<TDocument> _dbCollection;
    protected readonly IDocumentMapper<TDomain, TDocument> _mapper;

    protected BaseRepository(
        IMongoDBContext context,
        IDocumentMapper<TDomain, TDocument> mapper,
        ILogger<BaseRepository<TDomain, TDocument>> logger)
    {
        _mongoContext = context;
        _logger = logger;
        _mapper = mapper;

        var mongoCollection = typeof(TDomain).Name + "s";
        _logger.LogTrace("Getting collection {mongoCollection}", mongoCollection);

        _dbCollection = _mongoContext.GetCollection<TDocument>(mongoCollection);
    }

    public async Task Create(TDomain obj)
    {
        if (obj == null)
        {
            throw new ArgumentNullException(typeof(TDocument).Name + " object is null");
        }

        await _dbCollection.InsertOneAsync(_mapper.ToDocument(obj));
    }

    public void Delete(string id)
    {
        var objectId = new ObjectId(id);
        _dbCollection.DeleteOneAsync(Builders<TDocument>.Filter.Eq("_id", objectId));

    }

    public async Task<TDomain> Get(string id)
    {
        var objectId = new ObjectId(id);

        FilterDefinition<TDocument> filter = Builders<TDocument>.Filter.Eq("_id", objectId);

        return _mapper.FromDocument(await(await _dbCollection.FindAsync(_ => _.Id == id)).FirstOrDefaultAsync());
    }

    public async Task<List<TDomain>> Get(int pageNumber, int pageSize)
    {
        // Определение фильтра (например, пустой фильтр для получения всех документов)
        var filter = Builders<TDocument>.Filter.Empty;

        // Определение сортировки по _id в порядке возрастания
        var sort = Builders<TDocument>.Sort.Ascending("_id");

        // Запрос с фильтром и лимитом для получения страницы
        var result = _dbCollection
            .Find(filter)
            .Sort(sort)
            .Skip(pageNumber * pageSize)
            .Limit(pageSize);

        return  (await result.ToListAsync())
            .Select(_ => _mapper.FromDocument(_))
            .ToList();
    }

    public void Update(TDomain obj)
    {
        _dbCollection
            .ReplaceOneAsync(Builders<TDocument>.Filter.Eq("_id", obj.Id), _mapper.ToDocument(obj));
    }
}