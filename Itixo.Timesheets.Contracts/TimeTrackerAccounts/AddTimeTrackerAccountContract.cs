using Itixo.Timesheets.Domain;
using Itixo.Timesheets.Domain.TimeTrackers;
using MediatR;

namespace Itixo.Timesheets.Contracts.TimeTrackerAccounts;

public class AddTimeTrackerAccountContract : IRequest<AddOrUpdateTimeTrackerAccountResult>, TimeTrackerAccount.ITimeTrackerAccountModifiable
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string ExternalId { get; set; }
    public string Ip { get; set; }
    public string CurrentIdentityExternalId { get; set; }
    public int TimeTrackerId { get; set; }
}
