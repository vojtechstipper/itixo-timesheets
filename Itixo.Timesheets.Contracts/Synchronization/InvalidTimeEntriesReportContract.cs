using System;
using System.Collections.Generic;
using AutoMapper;
using Itixo.Timesheets.Domain.Synchronization;
using Itixo.Timesheets.Shared.Abstractions;

namespace Itixo.Timesheets.Contracts.Synchronization;

public class InvalidTimeEntriesReportContract : IMapFrom<InvalidTimeEntriesReport>
{
    public string ReceiverEmailAddress { get; set; }
    public DateTimeOffset LastReceivedTime { get; set; }
    public List<long> ExternalIds { get; set; }

    public void Mapping(Profile profile)
        => profile.CreateMap<InvalidTimeEntriesReport, InvalidTimeEntriesReportContract>();
}
