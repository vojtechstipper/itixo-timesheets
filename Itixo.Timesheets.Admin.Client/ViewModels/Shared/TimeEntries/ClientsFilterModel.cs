using Itixo.Timesheets.Admin.Client.ApiServices.TimeEntries;
using Itixo.Timesheets.Contracts.Clients;

namespace Itixo.Timesheets.Admin.Client.ViewModels.Shared.TimeEntries;

public interface IClientsFilterModel
{
    Task LoadClients();
    List<int> GetSelectedClientIds();
    List<ClientListContract> Clients { get; set; }
    List<ClientListContract> SelectedClients { get; set; }
}

public class ClientsFilterModel : IClientsFilterModel
{
    private readonly ITimeEntriesApiService timeEntriesApiService;

    public ClientsFilterModel(ITimeEntriesApiService timeEntriesApiService)
    {
        this.timeEntriesApiService = timeEntriesApiService;
    }

    public List<ClientListContract> Clients { get; set; } = new List<ClientListContract>();
    public List<ClientListContract> SelectedClients { get; set; } = new List<ClientListContract>();

    public async Task LoadClients() => Clients = (await timeEntriesApiService.GetClientsAsync()).Value.ToList();
    public List<int> GetSelectedClientIds() => SelectedClients.Select(s => s.Id).ToList();
}
