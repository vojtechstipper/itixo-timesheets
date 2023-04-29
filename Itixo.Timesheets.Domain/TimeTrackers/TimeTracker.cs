using Itixo.Timesheets.Shared.Abstractions;
using Itixo.Timesheets.Shared.Enums;

namespace Itixo.Timesheets.Domain.TimeTrackers;

public class TimeTracker : IEntity<int>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public TimeTrackerType Type { get; set; }

    public static TimeTracker Create(string name)
    {
        return new TimeTracker {Name = name};
    }
}
