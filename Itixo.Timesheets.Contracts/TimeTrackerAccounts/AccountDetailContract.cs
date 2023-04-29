using AutoMapper;
using Itixo.Timesheets.Domain;
using Itixo.Timesheets.Domain.TimeTrackers;
using Itixo.Timesheets.Shared.Abstractions;

namespace Itixo.Timesheets.Contracts.TimeTrackerAccounts;

public class AccountDetailContract : IMapFrom<TimeTrackerAccount>
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string ExternalId { get; set; }
    public void Mapping(Profile profile) => profile.CreateMap<TimeTrackerAccount, AccountDetailContract>();
}
