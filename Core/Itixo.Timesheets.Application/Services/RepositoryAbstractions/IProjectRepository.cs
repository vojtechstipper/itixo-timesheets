using Itixo.Timesheets.Contracts.Projects;
using Itixo.Timesheets.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Itixo.Timesheets.Application.Services.RepositoryAbstractions;

public interface IProjectRepository : IEntityRepository<Project, int>
{
    void UpdateProject(Project itixoProject, string projectName);
    Task InsertProjectAsync(long togglId, string projectName);
    Task<ProjectDetailContract> GetProjectAsync(long toggleProjectId);
    Task<IEnumerable<ProjectListContract>> GetProjectsAsync();
    Task<IEnumerable<T>> GetProjectsAsync<T>();
    Task AddOrUpdateProjectAsync(ProjectContract contract);
    Task<string> RemoveProjectAsync(long togglProjectId);
    Task<IEnumerable<T>> GetByClientIdsOrAllAsync<T>(IEnumerable<int> clientIds);
    Task<Project> GetProjectEntityAsync(long externalId);
    Task<Project> GetByIdAsync(int id);
    Task<bool> AnyAsync(int id);
    Task<ProjectContract> GetByNameAsync(string name);
}
