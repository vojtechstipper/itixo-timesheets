using AutoMapper;
using Itixo.Timesheets.Domain;
using Itixo.Timesheets.Shared.Abstractions;

namespace Itixo.Timesheets.Contracts.Workspaces;

public class WorkspaceListContract : IMapFrom<Workspace>
{
    public int Id { get; set; }
    public int ExternalId { get; set; }
    public string Name { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Workspace, WorkspaceListContract>();
    }
}
