using FluentValidation;
using FluentValidation.Results;
using FluentValidation.Validators;
using Itixo.Timesheets.Contracts.Invoices;
using Itixo.Timesheets.Contracts.TimeEntries.Approveds;
using Itixo.Timesheets.Shared.Extensions;
using Itixo.Timesheets.Shared.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Itixo.Timesheets.Application.Invoices.AssignTimeEntriesToInvoiceCommand;

public class InvoiceAssignmentParameterValidator : AbstractValidator<SummaryTimeEntriesInvoiceAssignmentParameter>
{
    private readonly IInvoiceAssignmentParameterValidatorDataProvider invoiceAssignmentParameterValidatorDataProvider;

    public InvoiceAssignmentParameterValidator(IInvoiceAssignmentParameterValidatorDataProvider invoiceAssignmentParameterValidatorDataProvider)
    {
        this.invoiceAssignmentParameterValidatorDataProvider = invoiceAssignmentParameterValidatorDataProvider;
        RuleFor(x => x).CustomAsync(DataIntegrityValidateAsync);
    }

    private async Task DataIntegrityValidateAsync(SummaryTimeEntriesInvoiceAssignmentParameter parameter, ValidationContext<SummaryTimeEntriesInvoiceAssignmentParameter> validationContext, CancellationToken token)
    {
        var maximumOfSubstractionAllowedValue = TimeSpan.FromSeconds(59);

        parameter.ToDate = parameter.ToDate.GetDateWithMaximumTime();
        parameter.FromDate = parameter.FromDate.GetDateWithMinimumTime();

        List<TimeEntriesDurationContract> approvedDurationDtos = await invoiceAssignmentParameterValidatorDataProvider.GetApprovedTimeEntryDurationContracts(parameter, token);

        TimeSpan approvesDurationTimeSpan = TimeSpanExtensions.Sum(approvedDurationDtos.Select(s => s.Duration).ToArray());

         if(approvesDurationTimeSpan.Subtract(parameter.SummaryDurationApproves.FromCustomHoursFormat()) > maximumOfSubstractionAllowedValue )
        {
            ValidationFailure validationFailure = CreateIntegrityValidationFailure(nameof(SummaryTimeEntriesInvoiceAssignmentParameter.SummaryDurationApproves));
            validationContext.AddFailure(validationFailure);
            return;
        }

         List<TimeEntriesDurationContract> draftDurationDtos = await invoiceAssignmentParameterValidatorDataProvider.GetDraftedTimeEntriesDurationContracts(parameter, token);
        TimeSpan draftsDurationTimeSpan = TimeSpanExtensions.Sum(draftDurationDtos.Select(s => s.Duration).ToArray());

        if (draftsDurationTimeSpan.Subtract(parameter.SummaryDurationDrafts.FromCustomHoursFormat()) > maximumOfSubstractionAllowedValue )
        {
            ValidationFailure validationFailure = CreateIntegrityValidationFailure(nameof(SummaryTimeEntriesInvoiceAssignmentParameter.SummaryDurationDrafts));
            validationContext.AddFailure(validationFailure);
            return;
        }

        List<TimeEntriesDurationContract> bansDurationDtos = await invoiceAssignmentParameterValidatorDataProvider.GetBannedTimeEntriesDurationContracts(parameter, token);
        TimeSpan bansDurationToTimeSpan = TimeSpanExtensions.Sum(bansDurationDtos.Select(s => s.Duration).ToArray());

        if (bansDurationToTimeSpan.Subtract(parameter.SummaryDurationBans.FromCustomHoursFormat()) > maximumOfSubstractionAllowedValue)
        {
            ValidationFailure validationFailure = CreateIntegrityValidationFailure(nameof(SummaryTimeEntriesInvoiceAssignmentParameter.SummaryDurationBans));
            validationContext.AddFailure(validationFailure);
            return;
        }

        var allTimeEntries = approvedDurationDtos.Concat(draftDurationDtos).ToList();
        TimeSpan allTimeEntriesDurationToTimeSpan = TimeSpanExtensions.Sum(allTimeEntries.Select(s => s.Duration).ToArray());

        if (allTimeEntriesDurationToTimeSpan.Subtract(parameter.SummaryDurationAllEntries.FromCustomHoursFormat()) > maximumOfSubstractionAllowedValue)
        {
            ValidationFailure validationFailure = CreateIntegrityValidationFailure(nameof(SummaryTimeEntriesInvoiceAssignmentParameter.SummaryDurationAllEntries));
            validationContext.AddFailure(validationFailure);
        }
    }

    private ValidationFailure CreateIntegrityValidationFailure(string nameOfProperty)
    {
        return new ValidationFailure(nameOfProperty, nameof(Validations.ReportsInvoiceAssignment_ValidationMessage_Data_Was_Changed));
    }
}
