using Itixo.Timesheets.Contracts.Projects;
using Itixo.Timesheets.Shared.Abstractions;
using Itixo.Timesheets.Shared.Exceptions;
using Microsoft.Extensions.Configuration;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using TogglSyncShared.ApiInterfaces;
using TogglSyncShared.DataObjects;
using TogglSyncShared.Refit;
using Task = System.Threading.Tasks.Task;

namespace TogglSyncShared;

public interface ITogglProjectSynchronizer : IService
{
    Task Synchronize(string apiToken);
    List<ProjectContract> SynchronizedProjects { get; }
}

public class TogglProjectSynchronizer : ITogglProjectSynchronizer
{
    private readonly IApiConnectorFactory apiConnectorFactory;
    private readonly IConfiguration configuration;

    public List<ProjectContract> SynchronizedProjects { get; } = new List<ProjectContract>();

    public TogglProjectSynchronizer(IApiConnectorFactory apiConnectorFactory, IConfiguration configuration)
    {
        this.apiConnectorFactory = apiConnectorFactory;
        this.configuration = configuration;
    }

    public async Task Synchronize(string apiToken)
    {
        var authHeader = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(apiToken + ":" + "api_token"));

        var refitSettings = new RefitSettings()
        {
            AuthorizationHeaderValueGetter = () => Task.FromResult(authHeader)
        };
        var togglAPI = RestService.For<ITogglAPI>("https://api.track.toggl.com/api/v9", refitSettings);
        var togglTogglProjects = await togglAPI.GetProjects();

        foreach (var project in togglTogglProjects)
        {
            if (project.ClientId == null)
            {
                throw new NotFoundException("Project has to have assigned Client");
            }

            Client togglClient = await togglAPI.GetClient(project.WorkspaceId ?? 0, project.ClientId ?? 0);

            var projectContract = new ProjectContract
            {
                Name = project.Name,
                ExternalClientId = togglClient.Id ?? throw new NotFoundException("Client not found in Toggl Api"),
                ClientName = togglClient.Name,
                ExternalId = (long?)project.Id ?? throw new NotFoundException("TogglProject not found in Toggl Api")
            };

            SynchronizedProjects.Add(projectContract);

            IProjectsApi projectsApi = apiConnectorFactory.CreateApiConnector<IProjectsApi>();
            await projectsApi.AddOrUpdate(projectContract);
        }

        await AddFictionalProject();
    }

    private async Task AddFictionalProject()
    {
        IProjectsApi projectsApi = apiConnectorFactory.CreateApiConnector<IProjectsApi>();
        ProjectContract fictionalProject = await projectsApi.GetByName(configuration["FictionalProject:ProjectName"]);
        if (fictionalProject == null)
        {
            fictionalProject = new ProjectContract
            {
                Name = configuration["FictionalProject:ProjectName"],
                ExternalClientId = Convert.ToInt32(configuration["FictionalProject:ClientExternalId"]),
                ExternalId = Convert.ToInt64(configuration["FictionalProject:ExternalId"])
            };
            await projectsApi.AddOrUpdate(fictionalProject);
        };
    }
}
