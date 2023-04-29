using System;
using AutoMapper;
using Itixo.Timesheets.Domain.TimeEntries;
using Itixo.Timesheets.Shared.Abstractions;

namespace Itixo.Timesheets.Contracts.TimeEntries.Approveds;

public class TimeEntriesDurationContract : IMapFrom<TimeEntry>
{
    public TimeSpan Duration { get; set; }
    public int TimeTrackerAccountId { get; set; }
    public long? ExternalId { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<TimeEntry, TimeEntriesDurationContract>()
            .ForMember(dest => dest.ExternalId, act => act.MapFrom(from => from.TimeEntryParams.ExternalId))
            .ForMember(dest => dest.Duration, act => act.MapFrom(from => from.TimeEntryParams.Duration));
    }
}
