using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Itixo.Timesheets.Contracts;
using Itixo.Timesheets.Contracts.Projects;

namespace Itixo.Timesheets.Admin.Client.ViewModels;

public partial class ReportsViewModel
{
    public List<ProjectBaseContract> Projects { get; set; } = new List<ProjectBaseContract>();
    public List<ProjectBaseContract> SelectedProjects { get; set; } = new List<ProjectBaseContract>();
    public string SelectedProjectIds => string.Join(",",SelectedProjects.Select(s => s.Id));

    public async Task LoadProjects()
    {
        SelectedProjects.Clear();
        List<int> selectedClientIds = GetSelectedClientIds();
        Projects = (await reportsApiService.GetProjectsAsync(selectedClientIds)).Value.ToList();
    }

    private List<int> GetSelectedProjectIds() => SelectedProjects.Select(s => s.Id).ToList();
}
