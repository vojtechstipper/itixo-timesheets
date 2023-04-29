using Itixo.Timesheets.Notificator.Lib;
using Itixo.Timesheets.Shared.Services;
using Microsoft.Extensions.DependencyInjection;
using SlimMessageBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using TogglSyncShared.ApiInterfaces;

namespace TogglSyncShared.InvalidRecords;

public class InvalidTogglRercordsReporter : IInvalidTogglRercordsReporter
{
    private readonly IDateTimeProvider dateTimeProvider;
    private readonly IServiceScopeFactory serviceScopeFactory;
    private readonly ISynchronizationApi synchronizationApi;

    public InvalidTogglRercordsReporter(IDateTimeProvider dateTimeProvider, IApiConnectorFactory apiConnectorFactory, IServiceScopeFactory serviceScopeFactory)
    {
        this.dateTimeProvider = dateTimeProvider;
        this.serviceScopeFactory = serviceScopeFactory;
        synchronizationApi = apiConnectorFactory.CreateApiConnector<ISynchronizationApi>();
    }

    public async Task ReportInvalidRecords(List<ReportersInvalidRecord> invalidRecords, string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return;
        }

        if (!IsEmailValid(email))
        {
            return;
        }

        GetInvalidTimeEntriesReportsResponse lastInvalidReport
            = await synchronizationApi.GetInvalidReport(new GetInvalidTimeEntriesReportsParameter { ReceiverEmailAddress = email });

        if (lastInvalidReport?.InvalidTimeEntriesReport == null && invalidRecords.Count == 0)
        {
            return;
        }

        if (lastInvalidReport?.InvalidTimeEntriesReport == null && invalidRecords.Count > 0)
        {
            await SendAndCreateReport(invalidRecords, email);
            return;
        }

        IEnumerable<long> newInvalidRecordIds = invalidRecords
            .Select(s => s.ExternalId)
            .Except(lastInvalidReport.InvalidTimeEntriesReport.ExternalIds);

        if (newInvalidRecordIds.Any())
        {
            await SendAndCreateReport(invalidRecords, email);
            return;
        }

        if (lastInvalidReport.InvalidTimeEntriesReport.LastReceivedTime.AddDays(3) > DateTimeOffset.Now)
        {
            return;
        }

        IEnumerable<long> stillNotFixedRecords = invalidRecords
            .Select(s => s.ExternalId)
            .Union(lastInvalidReport.InvalidTimeEntriesReport.ExternalIds)
            .ToList();

        if (stillNotFixedRecords.Any())
        {
            invalidRecords.RemoveAll(x => !stillNotFixedRecords.Contains(x.ExternalId));
            await SendAndCreateReport(invalidRecords, email);
        }
    }

    private async Task SendAndCreateReport(List<ReportersInvalidRecord> invalidRecords, string email)
    {
        if (invalidRecords.Any())
        {
            using var scope = serviceScopeFactory.CreateScope();
            var messageBus = scope.ServiceProvider.GetService<IMessageBus>();
            await SendEmail(messageBus, invalidRecords, email);
        }

        await UpdateListInvalidReport(invalidRecords, email);
    }

    private async Task UpdateListInvalidReport(List<ReportersInvalidRecord> invalidRecords, string email)
    {
        var command =
            new AddOrUpdateInvalidTimeEntriesReportCommand
            {
                LastReceivedTime = dateTimeProvider.Now,
                ReceiverEmailAddress = email,
                ExternalIds = invalidRecords.Select(s => s.ExternalId).ToList()
            };

        await synchronizationApi.AddOrUpdateInvalidReport(command);
    }

    private async Task SendEmail(IMessageBus messageBus, List<ReportersInvalidRecord> invalidRecords, string email) =>
        await messageBus
            .Publish(
            new NotificatorMessage
            {
                Data = new { InvalidRecords = invalidRecords },
                NotificationType = NotificationType.InvalidTimeEntriesReport,
                OutputType = OutputType.Email,
                Receiver = email
            });

    public bool IsEmailValid(string emailaddress)
    {
        try
        {
            new MailAddress(emailaddress);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }
}
