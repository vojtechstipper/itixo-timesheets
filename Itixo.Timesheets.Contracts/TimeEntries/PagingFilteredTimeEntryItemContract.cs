using AutoMapper;
using Itixo.Timesheets.Contracts.TimeEntries.Interfaces;
using Itixo.Timesheets.Domain.TimeEntries;
using Itixo.Timesheets.Shared.Abstractions;

namespace Itixo.Timesheets.Contracts.TimeEntries;

public class PagingFilteredTimeEntryItemContract : IFilteredTimeEntriesContract, IMapFrom<TimeEntry>
{
    public long? ExternalId { get; set; }
    public string Description { get; set; }
    public string ProjectName { get; set; }
    public int TimeTrackerAccountId { get; set; }
    public int ProjectId { get; set; }
    public string TaskName { get; set; }
    public string Username { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<TimeEntry, PagingFilteredTimeEntryItemContract>()
            .ForMember(dest => dest.Description, act => act.MapFrom(from => from.TimeEntryParams.Description))
            .ForMember(dest => dest.TimeTrackerAccountId, act => act.MapFrom(from => from.TimeTrackerAccountId))
            .ForMember(dest => dest.ProjectId, act => act.MapFrom(from => from.TimeEntryParams.Project.Id))
            .ForMember(dest => dest.ProjectName, act => act.MapFrom(from => from.TimeEntryParams.Project.Name))
            .ForMember(dest => dest.TaskName, act => act.MapFrom(from => from.TimeEntryParams.TaskName))
            .ForMember(dest => dest.ExternalId, act => act.MapFrom(from => from.TimeEntryParams.ExternalId))
            .ForMember(dest => dest.Username, act => act.MapFrom(from => from.TimeTrackerAccount.Username));
    }
}
