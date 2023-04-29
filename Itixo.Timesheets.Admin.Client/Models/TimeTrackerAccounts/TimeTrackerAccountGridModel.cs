using Itixo.Timesheets.Contracts.TimeTrackers;

namespace Itixo.Timesheets.Admin.Client.Models.TimeTrackerAccounts;

public class TimeTrackerAccountGridModel
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string ExternalId { get; set; }
    public TimeTrackerContract TimeTracker { get; set; }
    public string Email { get; set; }
}
