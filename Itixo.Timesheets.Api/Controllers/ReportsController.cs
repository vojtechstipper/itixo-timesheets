using Itixo.Timesheets.Application.TimeTrackerAccounts.ReportsQuery;
using Itixo.Timesheets.Contracts.TimeEntries;
using Itixo.Timesheets.Contracts.TimeTrackerAccounts;
using Itixo.Timesheets.Shared.Extensions;
using Itixo.Timesheets.Shared.Roles;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Itixo.Timesheets.Api.Controllers;

[Route("[controller]")]
[RolesAuthorize(new[] { RoleDefinition.TimeEntriesAdministrator, RoleDefinition.TimeEntriesUser })]
public class ReportsController : AppControllerBase
{
    private readonly IReportsQueryOrchestrator reportsQueryOrchestrator;

    public ReportsController(IReportsQueryOrchestrator reportsQueryOrchestrator)
    {
        this.reportsQueryOrchestrator = reportsQueryOrchestrator;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromBody] ReportsQueryFilter filter)
    {
        filter.FromDate = filter.FromDate.GetDateWithMinimumTime();
        filter.ToDate = filter.ToDate.GetDateWithMaximumTime();
        IEnumerable<AccountReportGridItemContract> reports = await reportsQueryOrchestrator.ExecuteAsync(filter);
        return Ok(reports);
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary([FromBody] ReportsQueryFilter filter)
    {
        filter.FromDate = filter.FromDate.GetDateWithMinimumTime();
        filter.ToDate = filter.ToDate.GetDateWithMaximumTime();
        IEnumerable<AccountReportGridItemContract> reports = await reportsQueryOrchestrator.ExecuteAsync(filter);

        var accountReportGridSummaryContract = new AccountReportGridSummaryContract
        {
            TotalSummaryAllEntries = new TimeSpan(reports.ToList().Sum(x => x.SummaryDurationAllEntries.Ticks)),
            TotalSummaryBansEntries = new TimeSpan(reports.ToList().Sum(x => x.SummaryDurationBans.Ticks)),
            TotalSummaryDraftsEntries = new TimeSpan(reports.ToList().Sum(x => x.SummaryDurationDrafts.Ticks)),
            TotalSummaryApprovesEntries = new TimeSpan(reports.ToList().Sum(x => x.SummaryDurationApproves.Ticks))
        };
        return Ok(accountReportGridSummaryContract);
    }
}
