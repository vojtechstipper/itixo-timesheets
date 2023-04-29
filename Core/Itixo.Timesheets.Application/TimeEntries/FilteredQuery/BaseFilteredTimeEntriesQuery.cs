using AutoMapper;
using Itixo.Timesheets.Application.Services.FilteredQueries;
using Itixo.Timesheets.Contracts.TimeEntries;
using Itixo.Timesheets.Contracts.TimeEntries.Interfaces;
using Itixo.Timesheets.Domain.Extensions.QueryFilters;
using Itixo.Timesheets.Domain.TimeEntries;
using Itixo.Timesheets.Shared.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Itixo.Timesheets.Application.TimeEntries.FilteredQuery;

public interface IBaseFilteredTimeEntriesQuery : IService
{
    Task<List<T>> GetFilteredResults<T>(TimeEntriesFilter filter) where T : class, IFilteredTimeEntriesContract, new();
}

public class BaseFilteredTimeEntriesQuery : IBaseFilteredTimeEntriesQuery
{
    private readonly IMapper mapper;
    private readonly IPersistenceQuery<TimeEntry, int> timeEntryQuery;

    private TimeEntriesFilter filter;

    public BaseFilteredTimeEntriesQuery(IMapper mapper, IPersistenceQuery<TimeEntry, int> timeEntryQuery)
    {
        this.mapper = mapper;
        this.timeEntryQuery = timeEntryQuery;
    }

    public async Task<List<T>> GetFilteredResults<T>(TimeEntriesFilter filter)
        where T : class, IFilteredTimeEntriesContract, new()
    {
        this.filter = filter;
        var results = new List<T>();

        var timeEntryDrafts = (await GetTimeEntryDraftsQueryable<T>()).ToList<T>();
        var timeEntryBans = (await GetTimeEntryBansQueryable<T>()).ToList<T>();
        var timeEntryPreDelete = (await GetTimeEntryPreDeleteQueryable<T>()).ToList<T>();
        var timeEntries = (await GetTimeEntriesQueryable<T>()).ToList<T>();

        if (filter.IsDraftedRequired)
        {
            results.AddRange(timeEntryDrafts);
        }

        if (filter.IsBannedRequired)
        {
            results.AddRange(timeEntryBans);
        }

        if (filter.IsPreDeleteRequired)
        {
            results.AddRange(timeEntryPreDelete);
        }

        if (filter.IsApprovedRequired)
        {
            results.AddRange(timeEntries);
        }

        if (!filter.IsBannedRequired && !filter.IsDraftedRequired && !filter.IsApprovedRequired && !filter.IsPreDeleteRequired)
        {
            return timeEntries.Concat(timeEntryBans).Concat(timeEntryDrafts).ToList();
        }

        return results;
    }

    private async Task<IEnumerable<T>> GetTimeEntryDraftsQueryable<T>()
    {
        IQueryable<TimeEntry> query = timeEntryQuery.GetQueryable()
            .Where(w => w.StateContext.IsDraft)
            .AsNoTracking()
            .Include(x => x.TimeEntryParams).ThenInclude(x => x.Project)
            .Include(x => x.StateContext)
            .Include(x => x.Invoice)
            .Include(x => x.TimeTrackerAccount);

        query = ApplyFilters(query);

        return await mapper.ProjectTo<T>(query).ToListAsync();
    }

    private async Task<IEnumerable<T>> GetTimeEntryBansQueryable<T>()
    {
        IQueryable<TimeEntry> query = timeEntryQuery.GetQueryable()
            .Where(w => w.StateContext.IsBan)
            .AsNoTracking()
            .Include(x => x.TimeEntryParams).ThenInclude(x => x.Project)
            .Include(x => x.StateContext)
            .Include(x => x.Invoice)
            .Include(x => x.TimeTrackerAccount);

        query = ApplyFilters(query);

        return await mapper.ProjectTo<T>(query).ToListAsync();
    }

    private async Task<IEnumerable<T>> GetTimeEntryPreDeleteQueryable<T>()
    {
        IQueryable<TimeEntry> query = timeEntryQuery.GetQueryable()
            .Where(w => w.StateContext.IsPredeleted)
            .AsNoTracking()
            .Include(x => x.TimeEntryParams).ThenInclude(x => x.Project)
            .Include(x => x.StateContext)
            .Include(x => x.Invoice)
            .Include(x => x.TimeTrackerAccount);

        query = ApplyFilters(query);

        return await mapper.ProjectTo<T>(query).ToListAsync();
    }

    private async Task<IEnumerable<T>> GetTimeEntriesQueryable<T>()
    {
        IQueryable<TimeEntry> query = timeEntryQuery.GetQueryable()
            .Where(w => w.StateContext.IsApproved)
            .AsNoTracking()
            .Include(x => x.TimeEntryParams).ThenInclude(x => x.Project)
            .Include(x => x.StateContext)
            .Include(x => x.Invoice)
            .Include(x => x.TimeTrackerAccount);

        query = ApplyFilters(query);

        return await mapper.ProjectTo<T>(query).ToListAsync();
    }

    private IQueryable<TimeEntry> ApplyFilters(IQueryable<TimeEntry> queryable)
    {
        if (filter.AccountIds.Any())
        {
            queryable = queryable.Where(item => filter.AccountIds.Contains(item.TimeTrackerAccountId));
        }

        if (filter.FromDate != default)
        {
            queryable = queryable.StopTimeGreaterOrEqual(filter.FromDate);
        }

        if (filter.ToDate != default)
        {
            queryable = queryable.StopTimeLessOrEqual(filter.ToDate);
        }

        if (filter.ProjectIds.Any())
        {
            queryable = queryable.OnlyInProjects(filter.ProjectIds);
        }

        if (filter.ClientIds.Any())
        {
            queryable = queryable.OnlyInClients(filter.ClientIds);
        }

        if (!string.IsNullOrWhiteSpace(filter.ProjectNameSearchText))
        {
            queryable = queryable.Where(item => item.TimeEntryParams.Project.Name.ToLower().Contains(filter.ProjectNameSearchText.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(filter.TaskNameSearchText))
        {
            queryable = queryable.Where(item => item.TimeEntryParams.TaskName.ToLower().Contains(filter.TaskNameSearchText.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(filter.DescriptionSearchText))
        {
            queryable = queryable.LikeDescription(filter.DescriptionSearchText);
        }

        if (!string.IsNullOrWhiteSpace(filter.UsernameSearchText))
        {
            queryable = queryable.Where(item => item.TimeTrackerAccount.Username.ToLower().Contains(filter.UsernameSearchText.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(filter.InvoiceNumberSearchText))
        {
            queryable = queryable.Where(item => item.Invoice.Number.ToLower().Contains(filter.InvoiceNumberSearchText.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(filter.InvoiceNumberSearchText))
        {
            queryable = queryable.Where(w => w.Invoice != null && w.Invoice.Number.Trim().ToLower().Contains(filter.InvoiceNumberSearchText.Trim().ToLower()));
        }

        return queryable;
    }
}
