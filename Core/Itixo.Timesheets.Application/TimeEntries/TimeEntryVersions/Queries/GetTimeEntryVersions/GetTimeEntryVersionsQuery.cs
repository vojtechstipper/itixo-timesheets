using MediatR;

namespace Itixo.Timesheets.Application.TimeEntries.TimeEntryVersions.Queries.GetTimeEntryVersions;

public class GetTimeEntryVersionsQuery : IRequest<GetTimeEntryVersionsQueryResult>
{
    public int TimeEntryId { get; set; }
}
