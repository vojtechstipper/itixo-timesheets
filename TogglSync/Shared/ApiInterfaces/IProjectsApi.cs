using Itixo.Timesheets.Contracts.Projects;
using Refit;
using System.Threading.Tasks;

namespace TogglSyncShared.ApiInterfaces;

public interface IProjectsApi
{
    [Post("/projects")]
    Task AddOrUpdate(ProjectContract projectContract);

    [Get("/projects/GetByName")]
    Task<ProjectContract> GetByName(string projectName);
}
