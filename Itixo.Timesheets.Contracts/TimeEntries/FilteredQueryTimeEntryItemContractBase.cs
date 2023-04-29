using Itixo.Timesheets.Shared.Services;
using System;
using System.Globalization;

namespace Itixo.Timesheets.Contracts.TimeEntries;

public class FilteredQueryTimeEntryItemContractBase
{
    public const string BanState = "Zamítnuté";
    public const string DraftState = "Ke schválení";
    public const string ApprovedState = "Schválené";
    public const string PreDeleteState = "Ke smazání";

    public DateTimeOffset LastModifiedDate { get; set; }
    public string Description { get; set; }
    public string Duration { get; set; }
    public DateTimeOffset StartTime { get; set; }
    public DateTimeOffset StopTime { get; set; }
    public string TaskName { get; set; }
    public string ProjectName { get; set; }
    public string ClientName { get; set; }
    public string Username { get; set; }
    public int TimeTrackerAccountId { get; set; }
    public int ProjectId { get; set; }
    public bool IsApproved { get; set; }
    public bool IsDraft { get; set; }
    public bool IsBan { get; set; }
    public bool IsPreDelete { get; set; }
    public virtual string InvoiceNumber { get; set; }
    public virtual string State { get; set; }
    public bool HasVersions { get; set; }

    public bool Equals(FilteredQueryTimeEntryItemContractBase equivalent)
    {
        return equivalent.IsApproved == IsApproved &&
               equivalent.IsBan == IsBan &&
               equivalent.IsDraft == IsDraft &&
               equivalent.IsPreDelete == IsPreDelete &&
               equivalent.Duration == Duration &&
               equivalent.Description == Description &&
               equivalent.StartTime == StartTime &&
               equivalent.StopTime == StopTime &&
               equivalent.TaskName == TaskName &&
               equivalent.ProjectName == ProjectName &&
               equivalent.TimeTrackerAccountId == TimeTrackerAccountId &&
               equivalent.ProjectId == ProjectId &&
               equivalent.InvoiceNumber == InvoiceNumber;
    }

    public DateTime ParseDate(string date)
    {
        return DateTime.ParseExact(date, CetDateConversionHelper.DisplayFormat, CultureInfo.InvariantCulture);
    }
}
