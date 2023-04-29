using AutoMapper;
using Itixo.Timesheets.Domain.TimeEntries;
using Itixo.Timesheets.Shared.Abstractions;
using System;

namespace Itixo.Timesheets.Contracts.TimeEntries.Approveds;

public class TimeEntryListContract : IMapFrom<TimeEntry>
{
    public int Id { get; set; }
    public long? TogglTimeEntryId { get; set; }
    public string Description { get; set; }
    public string Duration { get; set; }
    public DateTimeOffset StartTime { get; set; }
    public DateTimeOffset StopTime { get; set; }
    public string TaskName { get; set; }
    public long? TogglProjectId { get; set; }
    public string ProjectName { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<TimeEntry, TimeEntryListContract>()
            .ForMember(dest => dest.StartTime, act => act.MapFrom(from => from.TimeEntryParams.StartTime))
            .ForMember(dest => dest.StopTime, act => act.MapFrom(from => from.TimeEntryParams.StopTime))
            .ForMember(
                dest => dest.Duration,
                act => act.MapFrom(from => $"{from.TimeEntryParams.Duration.Hours:D2}:{from.TimeEntryParams.Duration.Minutes:D2}:{from.TimeEntryParams.Duration.Seconds:D2}"));
    }
}
