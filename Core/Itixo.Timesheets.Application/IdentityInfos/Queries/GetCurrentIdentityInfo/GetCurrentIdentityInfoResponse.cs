using AutoMapper;
using Itixo.Timesheets.Domain.Application;
using Itixo.Timesheets.Shared.Abstractions;

namespace Itixo.Timesheets.Application.IdentityInfos.Queries.GetCurrentIdentityInfo;

public class GetCurrentIdentityInfoResponse : IMapFrom<IdentityInfo>
{
    public string Email { get; set; }
    public bool DoesIdentityInfoExists { get; set; }

    public void Mapping(Profile profile) => profile.CreateMap<IdentityInfo, GetCurrentIdentityInfoResponse>()
        .ForMember(dest => dest.DoesIdentityInfoExists, act => act.MapFrom(_ => true));
}
