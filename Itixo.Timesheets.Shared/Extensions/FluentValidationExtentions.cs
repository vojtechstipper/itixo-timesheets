using System;
using FluentValidation;
using Itixo.Timesheets.Shared.Resources;

namespace Itixo.Timesheets.Shared.Extensions;

public static class FluentValidationExtentions
{
    public static IRuleBuilderOptions<T, DateTimeOffset> NotDefaultDateTimeOffSet<T>(
        this IRuleBuilder<T, DateTimeOffset> ruleBuilder)
    {
        return ruleBuilder.Must(dateTime => dateTime != default)
            .WithMessage(Validations.LogSyncCommandValidator_LogSyncCommandValidator_Date_cannot_be_default);
    }
}
