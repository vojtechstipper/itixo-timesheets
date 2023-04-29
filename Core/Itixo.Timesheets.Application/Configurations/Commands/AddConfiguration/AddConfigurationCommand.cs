using AutoMapper;
using Itixo.Timesheets.Application.Configurations.Commands.Behaviors;
using Itixo.Timesheets.Domain.Application;
using Itixo.Timesheets.Shared.Abstractions;
using Itixo.Timesheets.Shared.ConstantObjects;
using MediatR;
using MediatR.Extensions.AttributedBehaviors;

namespace Itixo.Timesheets.Application.Configurations.Commands.AddConfiguration;

[MediatRBehavior(typeof(ConfigurationKeyUniquenessValidator<AddConfigurationCommand, AddConfigurationResponse>))]
public class AddConfigurationCommand : IRequest<AddConfigurationResponse>, IConfigurationKeyUniquenessValidable, IMapFrom<Configuration>
{
    public string Key { get; set; }
    public string Value { get; set; }

    public static AddConfigurationCommand CreateStartSyncBusinessDaysAgoCommand(int startSyncBusinessDaysAgo)
    {
        return new AddConfigurationCommand
        {
            Key = ConfigurationConstants.StartSyncBusinessDaysAgo,
            Value = startSyncBusinessDaysAgo.ToString()
        };
    }

    public static AddConfigurationCommand CreateStopSyncBusinessDaysAgoCommand(int stopSyncBusinessDaysAgo)
    {
        return new AddConfigurationCommand
        {
            Key = ConfigurationConstants.StopSyncBusinessDaysAgo,
            Value = stopSyncBusinessDaysAgo.ToString()
        };
    }

    public void Mapping(Profile profile) => profile.CreateMap<AddConfigurationCommand, Configuration>();
}
