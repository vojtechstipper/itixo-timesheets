using Itixo.Timesheets.Domain.Application;

namespace Itixo.Timesheets.Domain.TimeTrackers;

public class IdentityTimeTrackerAccount
{
    public int TimeTrackerAccountId { get; set; }
    public TimeTrackerAccount TimeTrackerAccount { get; set; }
    public int IdentityInfoId { get; set; }
    public IdentityInfo IdentityInfo { get; set; }
}
