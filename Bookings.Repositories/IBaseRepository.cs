using Bookings.Domain;

namespace Bookings.Repositories;

public interface IBaseRepository<TEntity> where TEntity : BaseObject
{
    Task Create(TEntity obj);
    void Update(TEntity obj);
    void Delete(string id);
    Task<TEntity> Get(string id);
    Task<List<TEntity>> Get(int pageNumber, int pageSize);
}
