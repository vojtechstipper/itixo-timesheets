namespace Itixo.Timesheets.Contracts.TimeEntries.Interfaces;

public interface IFilteredTimeEntriesContract
{
    long? ExternalId { get; }
    string Description { get; }
    string ProjectName { get; }
    int TimeTrackerAccountId { get; }
    int ProjectId { get; }
    string TaskName { get; }
    string Username { get; }
}
