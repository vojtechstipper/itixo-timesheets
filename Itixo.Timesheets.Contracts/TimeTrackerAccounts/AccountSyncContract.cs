using System.Linq;
using AutoMapper;
using Itixo.Timesheets.Domain;
using Itixo.Timesheets.Domain.TimeTrackers;
using Itixo.Timesheets.Shared.Abstractions;
using Itixo.Timesheets.Shared.Enums;

namespace Itixo.Timesheets.Contracts.TimeTrackerAccounts;

public class AccountSyncContract : IMapFrom<TimeTrackerAccount>
{
    public string Email { get; set; }
    public string Username { get; set; }
    public string ExternalId { get; set; }
    public TimeTrackerType TimeTrackerType { get; set; }

    public void Mapping(Profile profile) => profile.CreateMap<TimeTrackerAccount, AccountSyncContract>()
        .ForMember(dest => dest.ExternalId, act => act.MapFrom(from => from.ExternalId))
        .ForMember(dest => dest.TimeTrackerType, act => act.MapFrom(from => from.TimeTracker.Type))
        .ForMember(dest => dest.Username, act => act.MapFrom(from => from.Username))
        .ForMember(dest => dest.Email, act => act.MapFrom(from
            => from.Identities.Any(f => !string.IsNullOrWhiteSpace(f.IdentityInfo.Email))
            ? from.Identities.First(f => !string.IsNullOrWhiteSpace(f.IdentityInfo.Email)).IdentityInfo.Email
            : from.Username));
}
