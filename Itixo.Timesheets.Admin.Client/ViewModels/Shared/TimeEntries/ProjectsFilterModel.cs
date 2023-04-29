using Itixo.Timesheets.Admin.Client.ApiServices.TimeEntries;
using Itixo.Timesheets.Contracts.Projects;

namespace Itixo.Timesheets.Admin.Client.ViewModels.Shared.TimeEntries;

public interface IProjectsFilterModel
{
    List<ProjectBaseContract> SelectedProjects { get; set; }
    List<ProjectBaseContract> Projects { get; set; }
    Task LoadProjects();
    List<int> GetSelectedProjectIds();
}

public class ProjectsFilterModel : IProjectsFilterModel
{
    private readonly ITimeEntriesApiService timeEntriesApiService;
    private readonly IClientsFilterModel clientsFilterModel;

    public ProjectsFilterModel(ITimeEntriesApiService timeEntriesApiService, IClientsFilterModel clientsFilterModel)
    {
        this.timeEntriesApiService = timeEntriesApiService;
        this.clientsFilterModel = clientsFilterModel;
    }
    public List<ProjectBaseContract> Projects { get; set; } = new List<ProjectBaseContract>();
    public List<ProjectBaseContract> SelectedProjects { get; set; } = new List<ProjectBaseContract>();

    public async Task LoadProjects()
    {
        SelectedProjects.Clear();
        List<int> selectedClientIds = clientsFilterModel.GetSelectedClientIds();
        Projects = (await timeEntriesApiService.GetProjectsAsync(selectedClientIds)).Value.ToList();
    }

    public List<int> GetSelectedProjectIds() => SelectedProjects.Select(s => s.Id).ToList();
}
