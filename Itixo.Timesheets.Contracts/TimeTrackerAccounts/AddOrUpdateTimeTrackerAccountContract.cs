using MediatR;

namespace Itixo.Timesheets.Contracts.TimeTrackerAccounts;

public class AddOrUpdateTimeTrackerAccountContract : IRequest<AddOrUpdateTimeTrackerAccountResult>
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string ExternalId { get; set; }
    public string Ip { get; set; }
    public string CurrentIdentityExternalId { get; set; }
    public int TimeTrackerId { get; set; }

    public AddTimeTrackerAccountContract CreateAddContract()
    {
        return new AddTimeTrackerAccountContract
        {
            Id = Id,
            CurrentIdentityExternalId = CurrentIdentityExternalId,
            ExternalId = ExternalId,
            Ip = Ip,
            Username = Username,
            TimeTrackerId = TimeTrackerId
        };
    }

    public UpdateTimeTrackerAccountContract CreateUpdateContract(int id)
    {
        return new UpdateTimeTrackerAccountContract
        {
            Id = id,
            CurrentIdentityExternalId = CurrentIdentityExternalId,
            ExternalId = ExternalId,
            Ip = Ip,
            Username = Username,
            TimeTrackerId = TimeTrackerId
        };
    }
}
