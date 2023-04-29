using AutoMapper;
using Itixo.Timesheets.Domain;
using Itixo.Timesheets.Domain.TimeTrackers;
using Itixo.Timesheets.Shared.Abstractions;
using Itixo.Timesheets.Shared.Enums;

namespace Itixo.Timesheets.Contracts.TimeTrackers;

public class TimeTrackerContract : IMapFrom<TimeTracker>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public TimeTrackerType Type { get; set; }
    public void Mapping(Profile profile) => profile.CreateMap<TimeTracker, TimeTrackerContract>();
}
