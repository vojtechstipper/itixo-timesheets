using Itixo.Timesheets.Shared.Abstractions;
using System.Linq;

namespace Itixo.Timesheets.Application.Services.FilteredQueries;

public interface IPersistenceQuery<out T, TKey>
where T : IEntity<TKey>
{
    IQueryable<T> GetQueryable();

}
