using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Itixo.Timesheets.Shared.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Itixo.Timesheets.Application.Services.RepositoryAbstractions;

public interface IEntityRepository<TEntity, TKey>
where TEntity : class, IEntity<TKey>
{
    IEnumerable<TEntity> GetAll();
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken token = default);
    TEntity GetById(TKey id);
    Task<TEntity> GetByIdAsync(TKey id, CancellationToken token = default);
    Task<TKey> InsertAsync(TEntity entity, CancellationToken token = default);
    Task UpdateAsync(TEntity entity, CancellationToken token = default);
    Task DeleteAsync(TKey id, CancellationToken token = default);
    Task DeleteAsync(TEntity entity, CancellationToken token = default);
    DbSet<TEntity> GetDbSet();
}
