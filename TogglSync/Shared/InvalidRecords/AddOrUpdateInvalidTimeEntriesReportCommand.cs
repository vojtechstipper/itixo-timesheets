using System;
using System.Collections.Generic;

namespace TogglSyncShared.InvalidRecords;

public class AddOrUpdateInvalidTimeEntriesReportCommand
{
    public string ReceiverEmailAddress { get; set; }
    public DateTimeOffset LastReceivedTime { get; set; }
    public List<long> ExternalIds { get; set; } = new List<long>();
}
