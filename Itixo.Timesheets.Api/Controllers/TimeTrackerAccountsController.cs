using AutoMapper;
using Itixo.Timesheets.Application.Services.RepositoryAbstractions;
using Itixo.Timesheets.Contracts.TimeTrackerAccounts;
using Itixo.Timesheets.Domain.TimeTrackers;
using Itixo.Timesheets.Shared.Roles;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Itixo.Timesheets.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TimeTrackerAccountsController : AppControllerBase
{
    private readonly ITimeTrackerAccountRepository timeTrackerAccountRepository;
    private readonly IMapper mapper;

    public TimeTrackerAccountsController(ITimeTrackerAccountRepository timeTrackerAccountRepository, IMapper mapper)
    {
        this.timeTrackerAccountRepository = timeTrackerAccountRepository;
        this.mapper = mapper;
    }

    [HttpGet("{username}")]
    [RolesAuthorize(new[] { RoleDefinition.TimeEntriesAdministrator, RoleDefinition.TimeEntriesUser })]
    public async Task<IActionResult> GetByUsername(string username)
    {
        AccountDetailContract result = await timeTrackerAccountRepository.GetUsersTogglAccountDetailsAsync(username);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [RolesAuthorize(new[] { RoleDefinition.TimeEntriesAdministrator, RoleDefinition.TimeEntriesUser })]
    public async Task<IActionResult> GetById(int id)
    {
        TimeTrackerAccount timeTrackerAccount = await timeTrackerAccountRepository.GetAsync(id);
        AccountDetailContract result = mapper.Map<AccountDetailContract>(timeTrackerAccount);
        return Ok(result);
    }

    [HttpGet]
    [Route("SyncContractList")]
    [RolesAuthorize(new[] { RoleDefinition.TimeEntriesAdministrator, RoleDefinition.SynchronizatorAccess })]
    public async Task<ActionResult<List<AccountSyncContract>>> GetAccountSyncContracts()
    {
        IEnumerable<AccountSyncContract> results = await timeTrackerAccountRepository.ListAsync<AccountSyncContract>();
        return Ok(results);
    }

    [HttpGet]
    [Route("OnlyApplicationAccounts")]
    [RolesAuthorize(new[] { RoleDefinition.TimeEntriesAdministrator, RoleDefinition.TimeEntriesUser })]
    public async Task<ActionResult<List<AccountBaseContract>>> GetOnlyApplicationAccounts()
    {
        IEnumerable<AccountBaseContract> results
            = await timeTrackerAccountRepository.OnlyApplicationAccountsAsync<AccountBaseContract>();
        return Ok(results);
    }

    [HttpGet]
    [RolesAuthorize(new[] { RoleDefinition.TimeEntriesAdministrator, RoleDefinition.TimeEntriesUser })]
    public async Task<ActionResult<List<AccountListContract>>> Get()
    {
        IEnumerable<AccountListContract> results = await timeTrackerAccountRepository.ListAsync<AccountListContract>();
        return Ok(results);
    }

    [HttpPost]
    [RolesAuthorize(new[] { RoleDefinition.TimeEntriesAdministrator, RoleDefinition.TimeEntriesUser })]
    public async Task<ActionResult<AddOrUpdateTimeTrackerAccountResult>> Add([FromBody] AddTimeTrackerAccountContract addTimeTrackerAccountContract)
    {
        return await Mediator.Send(addTimeTrackerAccountContract);
    }

    [HttpPut("{id}")]
    [RolesAuthorize(new[] { RoleDefinition.TimeEntriesAdministrator, RoleDefinition.TimeEntriesUser })]
    public async Task<ActionResult<AddOrUpdateTimeTrackerAccountResult>> Update(int id, [FromBody] UpdateTimeTrackerAccountContract updateTimeTrackerAccountContract)
    {
        updateTimeTrackerAccountContract.Id = id;
        return await Mediator.Send(updateTimeTrackerAccountContract);
    }

    [HttpPost]
    [Route("AddOrUpdate")]
    [RolesAuthorize(new[] { RoleDefinition.TimeEntriesAdministrator, RoleDefinition.TimeEntriesUser })]
    public async Task<ActionResult<AddOrUpdateTimeTrackerAccountResult>>
        AddOrUpdate([FromBody] AddOrUpdateTimeTrackerAccountContract addOrUpdateTimeTrackerAccountContract)
    {
        return await Mediator.Send(addOrUpdateTimeTrackerAccountContract);
    }

    [HttpDelete("{id}")]
    [RolesAuthorize(new[] { RoleDefinition.TimeEntriesAdministrator, RoleDefinition.TimeEntriesUser })]
    public async Task Delete(int id)
    {
        await timeTrackerAccountRepository.DeleteAsync(id);
    }
}
