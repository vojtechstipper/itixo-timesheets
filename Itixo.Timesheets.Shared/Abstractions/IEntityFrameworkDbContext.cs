using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Itixo.Timesheets.Shared.Abstractions;

public interface IEntityFrameworkDbContext
{
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    int SaveChanges();
    DatabaseFacade Database { get; }
}
