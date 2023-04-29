using Itixo.Timesheets.Application.Configurations.Commands.AddSyncDateRange;
using Itixo.Timesheets.Application.Configurations.Commands.UpdateSyncDateRange;
using Itixo.Timesheets.Application.Configurations.Queries.SyncDateRange;
using Itixo.Timesheets.Application.Services.RepositoryAbstractions;
using Itixo.Timesheets.Application.SyncHistory.Queries.ListQuery;
using Itixo.Timesheets.Application.Synchronization.Commands.AddOrUpdateInvalidReport;
using Itixo.Timesheets.Application.Synchronization.Queries.GetInvalidTimeEntriesReport;
using Itixo.Timesheets.Application.TimeEntries.Sync;
using Itixo.Timesheets.Contracts.Configurations;
using Itixo.Timesheets.Contracts.Sync;
using Itixo.Timesheets.Contracts.SyncHistory;
using Itixo.Timesheets.Contracts.TimeEntries.Approveds;
using Itixo.Timesheets.Shared.Roles;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Itixo.Timesheets.Api.Controllers;

public class SynchronizationController : AppControllerBase
{
    private readonly ITimeEntrySynchronizer timeEntrySynchronizer;
    private readonly ISyncSharedLockRepository syncSharedLockRepository;

    public SynchronizationController(ITimeEntrySynchronizer timeEntrySynchronizer, ISyncSharedLockRepository syncSharedLockRepository)
    {
        this.timeEntrySynchronizer = timeEntrySynchronizer;
        this.syncSharedLockRepository = syncSharedLockRepository;
    }

    [HttpGet]
    [RolesAuthorize(new[] { RoleDefinition.TimeEntriesAdministrator, RoleDefinition.SynchronizatorAccess })]
    public async Task<IActionResult> GetSyncDateRange()
    {
        SyncDateRangeQueryResponse response = await Mediator.Send(new SyncDateRangeQuery());
        return Ok(response);
    }

    [HttpPost]
    [RolesAuthorize(new[] { RoleDefinition.TimeEntriesAdministrator })]
    public async Task<IActionResult> AddSyncDateRange([FromBody] AddSyncDateRangeCommand command)
    {
        AddSyncDateRangeResponse response = await Mediator.Send(command);
        return Ok(response);
    }

    [HttpPut]
    [RolesAuthorize(new[] { RoleDefinition.TimeEntriesAdministrator })]
    public async Task<IActionResult> UpdateSyncDateRange([FromBody] UpdateSyncDateRangeCommand command)
    {
        await Mediator.Send(command);
        return Ok();
    }

    [HttpPost]
    [RolesAuthorize(new[] { RoleDefinition.TimeEntriesAdministrator, RoleDefinition.SynchronizatorAccess })]
    public async Task<ActionResult<LogSyncResult>> LogSync([FromBody] LogSyncCommand command)
    {
        LogSyncResult result = await Mediator.Send(command);
        return Ok(result);
    }

    [HttpGet]
    [RolesAuthorize(new[] { RoleDefinition.TimeEntriesAdministrator, RoleDefinition.SynchronizatorAccess })]
    public async Task<ActionResult<List<SyncLogRecordContract>>> GetLogRecords([FromBody] SyncLogRecordsFilter filter)
    {
        List<SyncLogRecordContract> result = await Mediator.Send(new SyncLogRecordsQuery(filter));
        return Ok(result);
    }

    [HttpPost]
    [RolesAuthorize(new[] { RoleDefinition.TimeEntriesAdministrator, RoleDefinition.SynchronizatorAccess })]
    public async Task Sync([FromBody] List<TimeEntryContract> timeEntries)
    {
        await timeEntrySynchronizer.Process(timeEntries);
    }

    [HttpPost]
    [RolesAuthorize(new[] { RoleDefinition.TimeEntriesAdministrator, RoleDefinition.SynchronizatorAccess })]
    public async Task SyncAndLog([FromBody] SyncWithLogParameter parameter)
    {
        await timeEntrySynchronizer.Process(parameter);
    }

    [HttpGet]
    [RolesAuthorize(new[] { RoleDefinition.TimeEntriesAdministrator, RoleDefinition.SynchronizatorAccess, RoleDefinition.TimeEntriesUser })]
    public async Task<ActionResult<SyncSharedLockContract>> GetLock()
    {
        return Ok(await syncSharedLockRepository.GetCurrent());
    }

    [HttpPost]
    [RolesAuthorize(new[] { RoleDefinition.TimeEntriesAdministrator, RoleDefinition.SynchronizatorAccess })]
    public async Task<IActionResult> Lock([FromBody] SharedLockLockingContract lockingContract)
    {
        await syncSharedLockRepository.LockSynchornization(lockingContract.Value);
        return Ok();
    }

    [HttpPost]
    [RolesAuthorize(new[] { RoleDefinition.TimeEntriesAdministrator, RoleDefinition.SynchronizatorAccess })]
    public async Task<IActionResult> Unlock([FromBody] SharedLockUnlockingContract unlockingContract)
    {
        await syncSharedLockRepository.UnlockSynchornization(unlockingContract.Value);
        return Ok();
    }

    [HttpGet]
    [RolesAuthorize(new[] { RoleDefinition.TimeEntriesAdministrator, RoleDefinition.SynchronizatorAccess })]
    public async Task<IActionResult> GetInvalidReport([FromBody] GetInvalidTimeEntriesReportsQuery query)
    {
        GetInvalidTimeEntriesReportsResponse response = await Mediator.Send(query);
        return Ok(response);
    }

    [HttpPost]
    [RolesAuthorize(new[] { RoleDefinition.TimeEntriesAdministrator, RoleDefinition.SynchronizatorAccess })]
    public async Task<IActionResult> AddOrUpdateInvalidReport([FromBody] AddOrUpdateInvalidTimeEntriesReportCommand command)
    {
        await Mediator.Send(command);
        return Ok();
    }
}
