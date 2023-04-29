using Itixo.Timesheets.Application.Services.RepositoryAbstractions;
using Itixo.Timesheets.Contracts.Clients;
using Itixo.Timesheets.Shared.Roles;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Itixo.Timesheets.Api.Controllers;

[Route("[controller]")]
[RolesAuthorize(new[] { RoleDefinition.TimeEntriesAdministrator, RoleDefinition.TimeEntriesUser})]
public class ClientsController : AppControllerBase
{
    private readonly IClientRepository clientRepository;

    public ClientsController(IClientRepository clientRepository)
    {
        this.clientRepository = clientRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        IEnumerable<ClientListContract> clients = await clientRepository.GetAllAsync<ClientListContract>();
        return Ok(clients);
    }
}
