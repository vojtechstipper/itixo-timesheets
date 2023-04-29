using System;
using Itixo.Timesheets.Domain.Abstractions;
using Itixo.Timesheets.Shared.Abstractions;

namespace Itixo.Timesheets.Domain;

public class Project : IEntity<int>, IImportable
{
    public int Id { get; set; }
    public long ExternalId { get; set; }
    public string Name { get; set; }
    public int ClientId { get; set; }
    public virtual Client Client { get; set; }
    public DateTimeOffset ImportedDate { get; set; }
}
