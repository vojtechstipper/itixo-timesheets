namespace Itixo.Timesheets.Notificator.Lib;

public class NotificatorMessage
{
    public NotificationType NotificationType { get; set; }
    public OutputType OutputType { get; set; }
    public object Data { get; set; }
    public string Receiver { get; set; }
}
