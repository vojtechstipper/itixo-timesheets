using Itixo.Timesheets.Application.Configurations.Commands.Behaviors;
using Itixo.Timesheets.Application.Configurations.Commands.UpdateSyncDateRange;
using Itixo.Timesheets.Shared.ConstantObjects;
using MediatR;
using MediatR.Extensions.AttributedBehaviors;

namespace Itixo.Timesheets.Application.Configurations.Commands.UpdateConfiguration;

[MediatRBehavior(typeof(ConfigurationKeyUniquenessValidator<UpdateConfigurationCommand, Unit>))]

public class UpdateConfigurationCommand : IRequest<Unit>, IConfigurationKeyUniquenessValidable
{
    public string Key { get; set; }
    public string Value { get; set; }

    public static UpdateConfigurationCommand CreateStartSyncBusinessDaysAgoCommand(UpdateSyncDateRangeCommand request)
    {
        return new UpdateConfigurationCommand
        {
            Key = ConfigurationConstants.StartSyncBusinessDaysAgo,
            Value = request.StartSyncBusinessDaysAgoValue.ToString()
        };
    }

    public static UpdateConfigurationCommand CreateStopSyncBusinessDaysAgoCommand(UpdateSyncDateRangeCommand request)
    {
        return new UpdateConfigurationCommand
        {
            Key = ConfigurationConstants.StopSyncBusinessDaysAgo,
            Value = request.StopSyncBusinessDaysAgoValue.ToString()
        };
    }
}
