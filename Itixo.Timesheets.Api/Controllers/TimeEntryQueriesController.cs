using Itixo.Timesheets.Application.TimeEntries.FilteredQuery;
using Itixo.Timesheets.Application.TimeEntries.FilteredQuery.GroupedQuery;
using Itixo.Timesheets.Contracts.TimeEntries;
using Itixo.Timesheets.Shared.Extensions;
using Itixo.Timesheets.Shared.Roles;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Itixo.Timesheets.Api.Controllers;

[Route("[action]")]
[RolesAuthorize(new[] { RoleDefinition.TimeEntriesAdministrator, RoleDefinition.TimeEntriesUser, RoleDefinition.TimesheetAccess })]
public class TimeEntryQueriesController : AppControllerBase
{
    private readonly IGroupedFilteredTimeEntriesProvider groupedFilteredTimeEntriesProvider;
    private readonly IPagingGroupedFilteredTimeEntriesCounter pagingGroupedFilteredTimeEntriesCounter;
    private readonly FilteredTimeEntriesQuery filteredTimeEntriesQuery;

    public TimeEntryQueriesController(
        IGroupedFilteredTimeEntriesProvider groupedFilteredTimeEntriesProvider,
        IPagingGroupedFilteredTimeEntriesCounter pagingGroupedFilteredTimeEntriesCounter,
        FilteredTimeEntriesQuery filteredTimeEntriesQuery)
    {
        this.groupedFilteredTimeEntriesProvider = groupedFilteredTimeEntriesProvider;
        this.pagingGroupedFilteredTimeEntriesCounter = pagingGroupedFilteredTimeEntriesCounter;
        this.filteredTimeEntriesQuery = filteredTimeEntriesQuery;
    }

    [HttpGet]
    public async Task<IActionResult> FilteredTimeEntries(TimeEntriesFilter filter)
    {
        filter.ToDate = filter.ToDate.GetDateWithMaximumTime();
        filter.FromDate = filter.FromDate.GetDateWithMinimumTime();
        IEnumerable<FilteredTimeEntryItemContract> results = await groupedFilteredTimeEntriesProvider.ExecuteAsync(filter);
        return Ok(results);
    }

    [HttpGet]
    public async Task<ActionResult<TimeEntriesGridPageInfoContract>> FilteredTimeEntriesPagingInfo(TimeEntriesFilter filter)
    {
        filter.ToDate = filter.ToDate.GetDateWithMaximumTime();
        filter.FromDate = filter.FromDate.GetDateWithMinimumTime();

        return Ok(new TimeEntriesGridPageInfoContract
        {
            RecordsCount = await pagingGroupedFilteredTimeEntriesCounter.ExecuteAsync(filter)
        });
    }

    [HttpGet]
    public async Task<IActionResult> FilteredPreDeleteTimeEntries(TimeEntriesFilter filter)
    {
        filter.ToDate = filter.ToDate.GetDateWithMaximumTime();
        filter.FromDate = filter.FromDate.GetDateWithMinimumTime();
        IEnumerable<FilteredTimeEntryItemContract> timeEntries = await filteredTimeEntriesQuery.ExecuteAsync(filter);
        return Ok(timeEntries);
    }

    [HttpGet]
    public async Task<IActionResult> ApprovedTimeEntries([FromQuery] DateTime fromDate, [FromQuery] DateTime toDate, [FromQuery] int projectId)
    {
        var filter = new TimeEntriesFilter();
        filter.FromDate = fromDate.GetDateWithMinimumTime();
        filter.ToDate = toDate.GetDateWithMaximumTime();
        filter.IsApprovedRequired = true;
        filter.ProjectIds = new List<int> { projectId };
        IEnumerable<FilteredTimeEntryItemContract> timeEntries = await filteredTimeEntriesQuery.ExecuteAsync(filter);
        return Ok(timeEntries);
    }
}
