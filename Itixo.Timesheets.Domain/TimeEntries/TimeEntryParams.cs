using System;
using System.Collections.Generic;
using Itixo.Timesheets.Shared.Abstractions;

namespace Itixo.Timesheets.Domain.TimeEntries;

public class TimeEntryParams : ValueObject
{
    public long? ExternalId { get; set; }
    public string Description { get; set; }
    public string TaskName { get; set; }
    public DateTimeOffset StartTime { get; set; }
    public DateTimeOffset StopTime { get; set; }
    public Project Project { get; set; }
    public TimeSpan Duration => StopTime - StartTime;
    public DateTimeOffset LastModifiedDate { get; set; } = DateTimeOffset.Now;

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return ExternalId;
        yield return Description;
        yield return TaskName;
        yield return StartTime;
        yield return StopTime;
        yield return Project;
        yield return LastModifiedDate;
    }
}
