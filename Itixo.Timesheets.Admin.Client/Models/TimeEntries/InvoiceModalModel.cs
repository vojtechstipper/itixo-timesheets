using Itixo.Timesheets.Contracts.Invoices;

namespace Itixo.Timesheets.Admin.Client.Models.TimeEntries;

public class InvoiceModalModel
{
    public bool IsDisplayed { get; set; }
    public bool IsNumberFormStateActive { get; set; }
    public bool IsShowResultsStateActive { get; set; }
    public string InvoiceNumber { get; set; }
    public List<TimeEntry> TimeEntries { get; set; } = new List<TimeEntry>();
    public InvoiceAssignmentResult Result { get; set; } = new InvoiceAssignmentResult();

    public void ToNumberFormState()
    {
        IsDisplayed = true;
        IsNumberFormStateActive = true;
    }

    public void ToShowResultsState()
    {
        IsNumberFormStateActive = false;
        IsShowResultsStateActive = true;
    }

    public void Close()
    {
        IsShowResultsStateActive = IsNumberFormStateActive = IsDisplayed = false;
        InvoiceNumber = "";
        TimeEntries = new List<TimeEntry>();
        Result = new InvoiceAssignmentResult();
    }

    public static TimeEntry Create(TimeEntryGridModel timeEntry)
    {
        return new TimeEntry { Id = timeEntry.Id, State = timeEntry.State };
    }

    public class TimeEntry
    {
        public int Id { get; set; }
        public string State { get; set; }
    }

}
