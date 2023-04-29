using AutoMapper;
using Itixo.Timesheets.Domain;
using Itixo.Timesheets.Domain.TimeTrackers;
using Itixo.Timesheets.Shared.Abstractions;

namespace Itixo.Timesheets.Contracts.TimeTrackerAccounts;

public class AccountBaseContract : IMapFrom<TimeTrackerAccount>
{
    public int Id { get; set; }
    public string Name { get; set; }

    public void Mapping(Profile profile) => profile.CreateMap<TimeTrackerAccount, AccountBaseContract>()
        .ForMember(dest => dest.Name, act => act.MapFrom(from => $"{from.Username}"));
}
