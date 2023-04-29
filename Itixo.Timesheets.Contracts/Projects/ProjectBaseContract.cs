using AutoMapper;
using Itixo.Timesheets.Domain;
using Itixo.Timesheets.Shared.Abstractions;

namespace Itixo.Timesheets.Contracts.Projects;

public class ProjectBaseContract : IMapFrom<Project>
{
    public int Id { get; set; }
    public string Name { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Project, ProjectBaseContract>();
    }
}
