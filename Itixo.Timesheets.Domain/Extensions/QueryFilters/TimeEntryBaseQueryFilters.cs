using System;
using System.Collections.Generic;
using System.Linq;
using Itixo.Timesheets.Domain.TimeEntries;

namespace Itixo.Timesheets.Domain.Extensions.QueryFilters;

public static class TimeEntryBaseQueryFilters
{
    public static IQueryable<TimeEntry> LikeDescription(this IQueryable<TimeEntry> queryable, string description)
    {
        return queryable.Where(item => item.TimeEntryParams.Description.ToLower().Contains(description.ToLower()));
    }

    /// <summary>
    /// .Include(x => x.Project) should be applied before use this filter
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="queryable"></param>
    /// <param name="projectIds"></param>
    /// <returns></returns>
    public static IQueryable<TimeEntry> OnlyInProjects(this IQueryable<TimeEntry> queryable, IEnumerable<int> projectIds)
    {
        return queryable.Where(item => projectIds.Contains(item.TimeEntryParams.Project.Id));
    }

    /// <summary>
    /// .Include(x => x.Project) should be applied before use this filter
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="queryable"></param>
    /// <param name="clientIds"></param>
    /// <returns></returns>
    public static IQueryable<TimeEntry> OnlyInClients(this IQueryable<TimeEntry> queryable, IEnumerable<int> clientIds)
    {
        return queryable.Where(item => clientIds.Contains(item.TimeEntryParams.Project.ClientId));
    }

    public static IQueryable<TimeEntry> OnlyInTimeTrackers(this IQueryable<TimeEntry> queryable, IEnumerable<int> timeTrackerIds)
    {
        return queryable.Where(item => timeTrackerIds.Contains(item.TimeTrackerAccountId));
    }

    public static IQueryable<TimeEntry> StopTimeGreaterOrEqual(this IQueryable<TimeEntry> queryable, DateTime fromDate)
    {
        var fromDateOffSet = new DateTimeOffset(fromDate);
        return queryable.Where(item => item.TimeEntryParams.StopTime >= fromDateOffSet);
    }

    public static IQueryable<TimeEntry> StopTimeLessOrEqual(this IQueryable<TimeEntry> queryable, DateTime toDate)
    {
        var toDateOffSet = new DateTimeOffset(toDate);
        return queryable.Where(item => item.TimeEntryParams.StopTime <= toDateOffSet);
    }
}
