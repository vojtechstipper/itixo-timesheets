using AutoMapper;
using Itixo.Timesheets.Shared.Abstractions;
using System;
using Itixo.Timesheets.Domain.TimeEntries;

namespace Itixo.Timesheets.Contracts.TimeEntries.Approveds;

public class TimeEntryContract : IMapFrom<TimeEntryParams>
{
    public long? ExternalTimeEntryId { get; set; }
    public string TimeTrackerExternalId { get; set; }
    public string Description { get; set; }
    public DateTimeOffset StartTime { get; set; }
    public DateTimeOffset StopTime { get; set; }
    public DateTimeOffset LastModifiedDate { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public string TaskName { get; set; }
    public long? ExternalProjectId { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<TimeEntryContract, TimeEntryParams>()
            .ForMember(dest => dest.Project, act => act.Ignore())
            .ForMember(dest => dest.ExternalId, act => act.MapFrom(from => from.ExternalTimeEntryId));
    }

    public bool EqualsTimeEntryParams(TimeEntryParams domainObject)
    {
        return string.Equals(Description, domainObject.Description)
               && string.Equals(TaskName, domainObject.TaskName)
               && StartTime.CompareTo(domainObject.StartTime) == 0
               && StopTime.CompareTo(domainObject.StopTime) == 0
               && (domainObject.Project?.ExternalId ?? 0) == ExternalProjectId;
    }
}
