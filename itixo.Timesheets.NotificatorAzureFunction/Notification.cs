using AsyncAwaitBestPractices;
using Itixo.Timesheets.Notificator.Lib;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace itixo.Timesheets.NotificatorAzureFunction;

public class Notification
{
    private readonly ILogger _logger;
    public Notification(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<Notification>();
    }

    [Function("Notification")]
    public void Run([ServiceBusTrigger("notification", Connection = "ServiceBusConnectionString")] NotificatorMessage message)
    {
        var notificationProcessor = new NotificationProcessor();
        notificationProcessor.ProcessServiceBusMessageAsync(message, _logger).SafeFireAndForget();
    }
}

public class NotificationProcessor
{
    private readonly EmailSender emailSender;

    public NotificationProcessor()
    {
        this.emailSender = new EmailSender();
    }

    public async Task ProcessServiceBusMessageAsync(NotificatorMessage message, ILogger log)
    {
        switch (message.OutputType)
        {
            case OutputType.Email:
                await ProcessEmailNotificationAsync(message, log);
                break;
            default:
                throw new ArgumentException("Unknown Output type");
        }
    }

    private async Task ProcessEmailNotificationAsync(NotificatorMessage message, ILogger log)
    {
        switch (message.NotificationType)
        {
            case NotificationType.InvalidTimeEntriesReport:
                await SendInvalidTimeEntriesReportAsync(message, log);
                break;
            default:
                throw new ArgumentException("Unknown notification type");
        }
    }

    private async Task SendInvalidTimeEntriesReportAsync(NotificatorMessage message, ILogger log)
    {
        string templateId = Environment.GetEnvironmentVariable("SendGridTemplatesInvalidTimeEntriesRecordTemplateId");
        string sender = Environment.GetEnvironmentVariable("SendGridSendersInvalidTimeEntriesRecordSender");
        await emailSender.Send(message.Receiver, sender, templateId, message.Data, log);
    }
}
