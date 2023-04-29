using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Itixo.Timesheets.Contracts;
using Itixo.Timesheets.Contracts.Clients;

namespace Itixo.Timesheets.Admin.Client.ViewModels;

public partial class ReportsViewModel
{
    public List<ClientListContract> Clients { get; set; } = new List<ClientListContract>();
    public List<ClientListContract> SelectedClients { get; set; } = new List<ClientListContract>();
    public string SelectedClientIds => string.Join(",",SelectedClients.Select(s => s.Id));
    
    private async Task LoadClients() => Clients = (await reportsApiService.GetClientsAsync()).Value.ToList();
    private List<int> GetSelectedClientIds() => SelectedClients.Select(s => s.Id).ToList();
}
