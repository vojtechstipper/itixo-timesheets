namespace Itixo.Timesheets.Contracts.Projects;

public class ProjectContract
{
    public long ExternalId { get; set; }
    public string Name { get; set; }
    public int ExternalClientId { get; set; }
    public string ClientName { get; set; }
}
