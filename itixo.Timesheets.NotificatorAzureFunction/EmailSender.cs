using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace itixo.Timesheets.NotificatorAzureFunction;

public class EmailSender
{

    /// <summary>
    /// Sending emails through SendGrid template
    /// </summary>
    /// <param name="emails">email address separated by semicolon, no spaces ("daniel@rusnok.com;rusnok@daniel.net...)</param>
    /// <param name="sender"></param>
    /// <param name="templateId"></param>
    /// <param name="templateData"></param>
    /// <param name="log"></param>
    /// <returns></returns>
    public async Task Send(string emails, string sender, string templateId, object templateData, ILogger log)
    {
        var msg = new SendGridMessage();

        msg.SetFrom(new EmailAddress(sender));

        var recipients = new List<EmailAddress>
        {
            new EmailAddress(sender)
        };

        foreach (string email in emails.Split(";"))
        {
            recipients.Add(new EmailAddress(email));
        }

        msg.AddTos(recipients);

        msg.SetTemplateId(templateId);

        msg.SetTemplateData(templateData);

        string apiKey = Environment.GetEnvironmentVariable("SendGridApiKey");
        var client = new SendGridClient(apiKey);
        Response response = await client.SendEmailAsync(msg);
        log.Log(LogLevel.Information, await response.Body.ReadAsStringAsync());
    }
}
