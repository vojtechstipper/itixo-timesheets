using Itixo.Timesheets.Shared.Abstractions;

namespace Itixo.Timesheets.Domain.Application;

public class Configuration : IEntity<int>
{
    public int Id { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }
}
