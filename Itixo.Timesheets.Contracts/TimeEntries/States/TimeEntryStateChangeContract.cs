using AutoMapper;
using Itixo.Timesheets.Domain.TimeEntries.States;
using Itixo.Timesheets.Shared.Abstractions;
using Itixo.Timesheets.Shared.Extensions;
using System;

namespace Itixo.Timesheets.Contracts.TimeEntries.States;

public class TimeEntryStateChangeContract : IMapFrom<TimeEntryStateChange>
{
    public DateTimeOffset When { get; set; }
    public string Who { get; set; }
    public string Why { get; set; }
    public string From { get; set; }
    public string To { get; set; }

    public void Mapping(Profile profile)
        => profile.CreateMap<TimeEntryStateChange, TimeEntryStateChangeContract>()
            .ForMember(dest => dest.When, act => act.MapFrom(from => from.CreatedDate))
            .ForMember(dest => dest.Who, act => act.MapFrom(from => from.ChangedByIdentity.Identifier))
            .ForMember(dest => dest.Why, act => act.MapFrom(from => from.Reason))
            .ForMember(dest => dest.From, act => act.MapFrom(from => @from.PreviousState.GetEnumDescription()))
            .ForMember(dest => dest.To, act => act.MapFrom(from => @from.NewState.GetEnumDescription()));
}
