using Itixo.Timesheets.Contracts.TimeEntries;
using Itixo.Timesheets.Shared.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Itixo.Timesheets.Domain.TimeEntries;

namespace Itixo.Timesheets.Application.TimeTrackerAccounts.ReportsQuery;

public interface IReportTimeEntryQuery : IService
{
    IQueryable<TimeEntry> GetQueryable(IReportsQueryFilter queryFilter);
    Task<IEnumerable<T>> ExecuteAsync<T>(IReportsQueryFilter queryFilter);
}
