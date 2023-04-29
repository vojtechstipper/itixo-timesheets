using MediatR;
using System;
using System.Collections.Generic;

namespace Itixo.Timesheets.Application.Synchronization.Commands.AddOrUpdateInvalidReport;

public class AddOrUpdateInvalidTimeEntriesReportCommand : IRequest<Unit>
{
    public string ReceiverEmailAddress { get; set; }
    public DateTimeOffset LastReceivedTime { get; set; }
    public List<long> ExternalIds { get; set; }
}
