using AutoMapper;
using Itixo.Timesheets.Domain.Application;
using Itixo.Timesheets.Shared.Abstractions;
using System;

namespace Itixo.Timesheets.Contracts.Configurations;

public class SyncSharedLockContract : IMapFrom<SyncSharedLock>
{
    public bool IsLocked { get; set; }
    public DateTimeOffset Start { get; set; }
    public DateTimeOffset? End { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<SyncSharedLock, SyncSharedLockContract>()
            .ForMember(dest => dest.IsLocked, act => act.MapFrom(from => from.IsLocked()));
    }
}
