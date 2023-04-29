namespace Itixo.Timesheets.Contracts.TimeEntries;

public interface ITimeEntriesQueryFilter : IReportsQueryFilter
{
    bool IncludeApproved { get; set; }
    bool IncludeDrafts { get; set; }
    bool IncludeBans { get; set; }
}
