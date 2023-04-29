using AutoMapper;
using Itixo.Timesheets.Application.Services.FilteredQueries;
using Itixo.Timesheets.Contracts.TimeEntries;
using Itixo.Timesheets.Domain.Extensions.QueryFilters;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Itixo.Timesheets.Domain.TimeEntries;

namespace Itixo.Timesheets.Application.TimeTrackerAccounts.ReportsQuery;

public class ReportTimeEntryQuery : IReportTimeEntryQuery
{
    private readonly IMapper mapper;
    private readonly IPersistenceQuery<TimeEntry, int> timeEntryQuery;

    public ReportTimeEntryQuery(IMapper mapper, IPersistenceQuery<TimeEntry, int> timeEntryQuery)
    {
        this.mapper = mapper;
        this.timeEntryQuery = timeEntryQuery;
    }

    public IQueryable<TimeEntry> GetQueryable(IReportsQueryFilter queryFilter)
    {
        IQueryable<TimeEntry> timeEntries = timeEntryQuery.GetQueryable().Include(x => x.TimeEntryParams.Project);

        if (queryFilter.ProjectIds.Any())
        {
            timeEntries = timeEntries.OnlyInProjects(queryFilter.ProjectIds);
        }

        if (queryFilter.ClientIds.Any())
        {
            timeEntries = timeEntries.OnlyInClients(queryFilter.ClientIds);
        }

        if (queryFilter.TimeTrackerIds.Any())
        {
            timeEntries = timeEntries.OnlyInTimeTrackers(queryFilter.TimeTrackerIds);
        }

        if (queryFilter.FromDate != default)
        {
            timeEntries = timeEntries.Where(item => item.TimeEntryParams.StopTime >= queryFilter.FromDate);
        }
         
        if (queryFilter.ToDate != default)
        {
            timeEntries = timeEntries.Where(item => item.TimeEntryParams.StopTime <= queryFilter.ToDate);
        }

        return timeEntries;
    }

    public async Task<IEnumerable<T>> ExecuteAsync<T>(IReportsQueryFilter queryFilter)
    {
        return await mapper.ProjectTo<T>(GetQueryable(queryFilter)).ToListAsync();
    }
}
