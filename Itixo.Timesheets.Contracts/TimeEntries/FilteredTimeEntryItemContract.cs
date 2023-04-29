using AutoMapper;
using Itixo.Timesheets.Contracts.TimeEntries.Interfaces;
using Itixo.Timesheets.Domain.TimeEntries;
using Itixo.Timesheets.Shared.Abstractions;
using Itixo.Timesheets.Shared.Extensions;
using System.Linq;

namespace Itixo.Timesheets.Contracts.TimeEntries;

public class FilteredTimeEntryItemContract : FilteredQueryTimeEntryItemContractBase, IMapFrom<TimeEntry>, IFilteredTimeEntriesContract
{
    public string GroupId { get; set; }
    public int Id { get; set; }
    public long? ExternalId { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<TimeEntry, FilteredTimeEntryItemContract>()
            .ForMember(dest => dest.LastModifiedDate, act => act.MapFrom(from => from.TimeEntryParams.LastModifiedDate))
            .ForMember(dest => dest.StartTime, act => act.MapFrom(from => from.TimeEntryParams.StartTime))
            .ForMember(dest => dest.StopTime, act => act.MapFrom(from => from.TimeEntryParams.StopTime))
            .ForMember(dest => dest.Username, act => act.MapFrom(from => from.TimeTrackerAccount.Username))
            .ForMember(dest => dest.ExternalId, act => act.MapFrom(from => from.TimeEntryParams.ExternalId))
            .ForMember(dest => dest.Description, act => act.MapFrom(from => from.TimeEntryParams.Description))
            .ForMember(dest => dest.ProjectName, act => act.MapFrom(from => from.TimeEntryParams.Project.Name))
            .ForMember(dest => dest.TaskName, act => act.MapFrom(from => from.TimeEntryParams.TaskName))
            .ForMember(dest => dest.IsApproved, act => act.MapFrom(from => from.StateContext.IsApproved))
            .ForMember(dest => dest.IsDraft, act => act.MapFrom(from => from.StateContext.IsDraft))
            .ForMember(dest => dest.IsBan, act => act.MapFrom(from => from.StateContext.IsBan))
            .ForMember(dest => dest.IsPreDelete, act => act.MapFrom(from => from.StateContext.IsPredeleted))
            .ForMember(dest => dest.Duration, act => act.MapFrom(from => from.TimeEntryParams.Duration.ToCustomHoursFormat()))
            .ForMember(dest => dest.HasVersions, act => act.MapFrom(from => from.TimeEntryVersions.Any()))
            .ForMember(dest => dest.ProjectId, act => act.MapFrom(from => from.TimeEntryParams.Project.Id))
            .ForMember(dest => dest.ClientName, act => act.MapFrom(from => from.TimeEntryParams.Project.Client.Name));
    }
}
