using AutoMapper;
using Itixo.Timesheets.Domain;
using Itixo.Timesheets.Shared.Abstractions;

namespace Itixo.Timesheets.Contracts.Workspaces;

public class WorkspaceDetailContract : IMapFrom<Workspace>
{
    public int TogglWorkspaceId { get; set; }
    public string Name { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Workspace, WorkspaceDetailContract>();
    }
}
