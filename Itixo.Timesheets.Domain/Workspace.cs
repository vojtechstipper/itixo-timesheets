using Itixo.Timesheets.Shared.Abstractions;

namespace Itixo.Timesheets.Domain;

public class Workspace : IEntity<int>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int ExternalId { get; set; }
}
