using AutoMapper;
using Itixo.Timesheets.Domain.TimeEntries;
using Itixo.Timesheets.Shared.Abstractions;
using System;

namespace Itixo.Timesheets.Contracts.TimeEntries.Versions;

public class TimeEntryVersionContract : IMapFrom<TimeEntryVersion>
{
    public int Id { get; set; }
    public string Description { get; set; }
    public string TaskName { get; set; }
    public DateTimeOffset StartTime { get; set; }
    public DateTimeOffset StopTime { get; set; }
    public string ProjectName { get; set; }
    public string Duration { get; set; }
    public DateTimeOffset LastModifiedDate { get; set; }
    public DateTimeOffset ImportedDate { get; set; }
    public string PreviousDescription { get; set; }
    public string PreviousTasName { get; set; }
    public DateTimeOffset PreviousStartTime { get; set; }
    public DateTimeOffset PreviousStopTime { get; set; }
    public string PreviousProjectName { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<TimeEntryVersion, TimeEntryVersionContract>()
            .ForMember(dest => dest.PreviousDescription, act => act.MapFrom(from => from.PreviousTimeEntryVersion.TimeEntryParams.Description))
            .ForMember(dest => dest.PreviousTasName, act => act.MapFrom(from => from.PreviousTimeEntryVersion.TimeEntryParams.TaskName))
            .ForMember(dest => dest.PreviousStartTime, act => act.MapFrom(from => from.PreviousTimeEntryVersion.TimeEntryParams.StartTime))
            .ForMember(dest => dest.PreviousStopTime, act => act.MapFrom(from => from.PreviousTimeEntryVersion.TimeEntryParams.StopTime))
            .ForMember(dest => dest.PreviousProjectName, act => act.MapFrom(from => from.PreviousTimeEntryVersion.TimeEntryParams.Project.Name))
            .ForMember(dest => dest.Description, act => act.MapFrom(from => from.TimeEntryParams.Description))
            .ForMember(dest => dest.TaskName, act => act.MapFrom(from => from.TimeEntryParams.TaskName))
            .ForMember(dest => dest.StartTime, act => act.MapFrom(from => from.TimeEntryParams.StartTime))
            .ForMember(dest => dest.StopTime, act => act.MapFrom(from => from.TimeEntryParams.StopTime))
            .ForMember(dest => dest.ProjectName, act => act.MapFrom(from => from.TimeEntryParams.Project.Name))
            .ForMember(dest => dest.LastModifiedDate, act => act.MapFrom(from => from.TimeEntryParams.LastModifiedDate))
            .ForMember(dest => dest.ImportedDate, act => act.MapFrom(from => from.ImportedDate))
            .ForMember(dest => dest.Duration, act => act.MapFrom(from =>
                $"{from.TimeEntryParams.Duration.Hours:D2}:{from.TimeEntryParams.Duration.Minutes:D2}:{from.TimeEntryParams.Duration.Seconds:D2}"));
    }
}
