using Itixo.Timesheets.Application.Services.RepositoryAbstractions;
using Itixo.Timesheets.Persistence;
using Itixo.Timesheets.Shared.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Itixo.Timesheets.Infrastructure.Repos;

public class AppRepositoryBase<TEntity, TKey> : IEntityRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{
    protected readonly IDbContext dbContext;

    public AppRepositoryBase(IDbContext dbContext)
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

    public virtual async Task<TKey> InsertAsync(TEntity entity, CancellationToken token = default)
    {
        EntityEntry<TEntity> entry = await GetDbSet().AddAsync(entity, token);
        await dbContext.SaveChangesAsync(token);
        return entry.Entity.Id;
    }

    public virtual Task UpdateAsync(TEntity entity, CancellationToken token = default)
    {
        GetDbSet().Update(entity);
        return dbContext.SaveChangesAsync(token);
    }

    public virtual async Task DeleteAsync(TKey id, CancellationToken token = default)
    {
        TEntity entity = await GetByIdAsync(id, token);
        await DeleteAsync(entity, token);
    }

    public virtual Task DeleteAsync(TEntity entity, CancellationToken token = default)
    {
        GetDbSet().Remove(entity);
        return dbContext.SaveChangesAsync(token);
    }

    public virtual DbSet<TEntity> GetDbSet()
    {
        return dbContext.Set<TEntity>();
    }
}
