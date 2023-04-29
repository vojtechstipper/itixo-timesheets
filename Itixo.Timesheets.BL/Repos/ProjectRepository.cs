using AutoMapper;
using Itixo.Timesheets.Application.Services.RepositoryAbstractions;
using Itixo.Timesheets.Contracts.Projects;
using Itixo.Timesheets.Domain;
using Itixo.Timesheets.Persistence;
using Itixo.Timesheets.Shared.Exceptions;
using Itixo.Timesheets.Shared.Resources;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace Itixo.Timesheets.Infrastructure.Repos;

public class ProjectRepository : AppRepositoryBase<Project, int>, IProjectRepository
{
    private readonly IMapper mapper;

    public ProjectRepository(IDbContext dbContext, IMapper mapper) : base(dbContext)
    {
        this.mapper = mapper;
    }

    public void UpdateProject(Project itixoProject, string projectName)
    {
        if (itixoProject.Name != projectName)
        {
            itixoProject.Name = projectName;
            dbContext.Projects.Update(itixoProject);
        }
    }

    public async Task InsertProjectAsync(long togglId, string projectName)
    {
        var itixoProject = new Project { Name = projectName, ExternalId = togglId };
        await dbContext.Projects.AddAsync(itixoProject);
    }

    public async Task<ProjectDetailContract> GetProjectAsync(long toggleProjectId)
    {
        Project projectEntity = await GetProjectEntityAsync(toggleProjectId);

        if (projectEntity == null)
        {
            throw new NotFoundException(
                string.Format(Texts.ProjectRepository_RemoveProjectAsync_Project_With_Id_Was_Not_Found, toggleProjectId));
        }

        return new ProjectDetailContract { TogglProjectId = projectEntity.ExternalId, Name = projectEntity.Name };
    }

    public async Task<Project> GetProjectEntityAsync(long externalId)
    {
        return await dbContext.Projects.FirstOrDefaultAsync(p => p.ExternalId == externalId);
    }

    public Task<Project> GetByIdAsync(int id) => dbContext.Projects.FirstOrDefaultAsync(f => f.Id == id);

    public async Task<ProjectContract> GetByNameAsync(string name) =>
        await dbContext.Projects.Where(f => f.Name == name)
        .Select(x => new ProjectContract
        {
            ClientName = x.Client.Name,
            ExternalId = x.ExternalId,
            Name = x.Name,
            ExternalClientId = x.Client.ExternalId
        }).FirstOrDefaultAsync();

    public Task<bool> AnyAsync(int id) => dbContext.Projects.AnyAsync(x => x.Id == id);

    public async Task<IEnumerable<ProjectListContract>> GetProjectsAsync()
    {
        return await dbContext.Projects
            .Select(s => new ProjectListContract
            {
                TogglProjectId = s.ExternalId,
                Name = s.Name
            }).ToListAsync();
    }

    public async Task<IEnumerable<T>> GetProjectsAsync<T>()
    {
        return await mapper.ProjectTo<T>(dbContext.Projects).ToListAsync();
    }

    public async Task AddOrUpdateProjectAsync(ProjectContract contract)
    {
        Project projectEntity = await dbContext.Projects.FirstOrDefaultAsync(p => p.ExternalId == contract.ExternalId);
        Client clientEntity = await dbContext.Clients.FirstOrDefaultAsync(c => c.ExternalId == contract.ExternalClientId);

        if (projectEntity == null)
        {
            await InsertProject(contract, clientEntity);
        }
        else
        {
            await UpdateProject(contract, projectEntity, clientEntity);
        }
    }

    private async Task UpdateProject(ProjectContract contract, Project projectEntity, Client client)
    {
        projectEntity.Name = contract.Name;

        if (client == null)
        {
            client = new Client { Name = contract.ClientName, ExternalId = contract.ExternalClientId };
        }
        else
        {
            projectEntity.Client = client;
        }

        if (client.Name != contract.ClientName)
        {
            client.Name = contract.ClientName;
            dbContext.Clients.Update(client);
        }

        dbContext.Projects.Update(projectEntity);
        await dbContext.SaveChangesAsync();
    }

    private async Task InsertProject(ProjectContract contract, Client clientEntity)
    {
        if (clientEntity == null)
        {
            clientEntity = new Client { Name = contract.ClientName, ExternalId = contract.ExternalClientId };
        }

        var projectEntity = new Project { Name = contract.Name, ExternalId = contract.ExternalId, Client = clientEntity };

        await dbContext.Projects.AddAsync(projectEntity);
        await dbContext.SaveChangesAsync();
    }

    public async Task<string> RemoveProjectAsync(long togglProjectId)
    {
        Project projectEntity = await dbContext.Projects.FirstOrDefaultAsync(p => p.ExternalId == togglProjectId);

        if (projectEntity == null)
        {
            throw new NotFoundException(string.Format(Texts.ProjectRepository_RemoveProjectAsync_Project_With_Id_Was_Not_Found, togglProjectId));
        }

        dbContext.Projects.Remove(projectEntity);
        await dbContext.SaveChangesAsync();
        return Texts.Repository_RemoveMessage_Record_Was_Successfully_Removed;
    }

    public async Task<IEnumerable<T>> GetByClientIdsOrAllAsync<T>(IEnumerable<int> clientIds)
    {
        var clientIdsList = clientIds.ToList();
        IQueryable<Project> projects = dbContext.Projects.Where(project => clientIdsList.Contains(project.ClientId) || !clientIdsList.Any());
        return await mapper.ProjectTo<T>(projects).ToListAsync();
    }
}
