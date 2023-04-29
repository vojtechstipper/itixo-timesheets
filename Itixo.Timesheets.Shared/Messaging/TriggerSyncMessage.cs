using System;

namespace Itixo.Timesheets.Shared.Messaging;

public class TriggerSyncMessage
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string IdentityExternalId { get; set; }
    public bool ManualRun { get; set; }

    public TriggerSyncMessage() { }
    public TriggerSyncMessage(DateTime startDate, DateTime endDate, string identityExternalId)
    {
        StartDate = startDate;
        EndDate = endDate;
        IdentityExternalId = identityExternalId;
    }
}
