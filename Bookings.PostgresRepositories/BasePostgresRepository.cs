using Bookings.Domain;
using Bookings.Domain.Repositories;
using Bookings.PostgresRepositories.Contexts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Bookings.PostgresRepositories;

public abstract class BasePostgresRepository<TDomain> : IBaseRepository<TDomain>
    where TDomain : BaseObject
{
    protected readonly BookingsDbContext _context;
    protected readonly ILogger<BasePostgresRepository<TDomain>> _logger;
    protected readonly DbSet<TDomain> _dbSet;

    protected BasePostgresRepository(
        BookingsDbContext context,
        ILogger<BasePostgresRepository<TDomain>> logger)
    {
        _context = context;
        _logger = logger;
        _dbSet = context.Set<TDomain>();
    }

    public virtual async Task Create(TDomain obj)
    {
        if (obj == null)
        {
            throw new ArgumentNullException(typeof(TDomain).Name + " object is null");
        }

        await _dbSet.AddAsync(obj);
        await _context.SaveChangesAsync();
    }

    public virtual async Task Delete(string id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public virtual async Task<TDomain?> Get(string id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<List<TDomain>> Get(int pageNumber, int pageSize)
    {
        return await _dbSet
            .OrderBy(e => e.Id)
            .Skip(pageNumber * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public virtual async Task Update(TDomain obj)
    {
        _dbSet.Update(obj);
        await _context.SaveChangesAsync();
    }
} 