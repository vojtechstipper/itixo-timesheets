using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Itixo.Timesheets.Shared.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Itixo.Timesheets.Infrastructure.Repos;

public class RepositoryBase<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{
    private readonly DbContext dbContext;

    public RepositoryBase(DbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public virtual IEnumerable<TEntity> GetAll()
    {
        return GetDbSet().ToList();
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken token = default)
    {
        return await GetDbSet().ToListAsync(token);
    }

    public virtual TEntity GetById(TKey id)
    {
        return GetDbSet().SingleOrDefault(e => e.Id.Equals(id));
    }

    public virtual Task<TEntity> GetByIdAsync(TKey id, CancellationToken token = default)
    {
        return GetDbSet().SingleOrDefaultAsync(e => e.Id.Equals(id), token);
    }

    public virtual async Task InsertAsync(TEntity entity, CancellationToken token = default)
    {
        await GetDbSet().AddAsync(entity, token);
        await dbContext.SaveChangesAsync(token);
    }

    public virtual Task UpdateAsync(TEntity entity, CancellationToken token = default)
    {
        GetDbSet().Update(entity);
        return dbContext.SaveChangesAsync(token);
    }

    public virtual async Task DeleteAsync(TKey id, CancellationToken token = default)
    {
        var entity = await GetByIdAsync(id, token);
        await DeleteAsync(entity, token);
    }

    public virtual Task DeleteAsync(TEntity entity, CancellationToken token = default)
    {
        GetDbSet().Remove(entity);
        return dbContext.SaveChangesAsync(token);
    }

    protected virtual DbSet<TEntity> GetDbSet()
    {
        return dbContext.Set<TEntity>();
    }
}