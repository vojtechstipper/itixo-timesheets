namespace Itixo.Timesheets.Users.Client.Models;

public class AccountFormModel
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string ExternalId { get; set; }
    public string Ip { get; set; }
    public int TimeTrackerId { get; set; }
    public string Email { get; set; }
}
