using System.Linq;
using Itixo.Timesheets.Application.Services.FilteredQueries;
using Itixo.Timesheets.Persistence;
using Itixo.Timesheets.Shared.Abstractions;

namespace Itixo.Timesheets.Infrastructure.FilteredQueries;

public class BasicEntityQuery<T, TKey> : IPersistenceQuery<T, TKey>
where T : class, IEntity<TKey>
{
    private readonly IDbContext dbContext;

    public BasicEntityQuery(IDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public IQueryable<T> GetQueryable()
    {
        return dbContext.Set<T>();
    }
}
