using Itixo.Timesheets.Shared.Abstractions;
using System;
using System.Collections.Generic;

namespace Itixo.Timesheets.Domain.Synchronization;

public class InvalidTimeEntriesReport : IEntity<int>
{
    public int Id { get; set; }
    public string ReceiverEmailAddress { get; set; }
    public DateTimeOffset LastReceivedTime { get; set; }
    public List<long> ExternalIds { get; set; }

    public static InvalidTimeEntriesReport Create(string email, in DateTimeOffset lastReceivedTime, List<long> externalIds)
    {
        return new InvalidTimeEntriesReport
        {
            ReceiverEmailAddress = email,
            LastReceivedTime = lastReceivedTime,
            ExternalIds = externalIds
        };
    }

    public void Update(string email, in DateTimeOffset lastReceivedTime, List<long> externalIds)
    {
        ReceiverEmailAddress = email;
        LastReceivedTime = lastReceivedTime;
        ExternalIds = externalIds;
    }
}
