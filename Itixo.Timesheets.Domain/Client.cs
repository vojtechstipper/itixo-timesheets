using System;
using System.Collections.Generic;
using Itixo.Timesheets.Domain.Abstractions;
using Itixo.Timesheets.Shared.Abstractions;

namespace Itixo.Timesheets.Domain;

public class Client : IEntity<int>, IImportable
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int ExternalId { get; set; }
    public DateTimeOffset ImportedDate { get; set; }
    public List<Project> Projects { get; set; } = new List<Project>();
}
