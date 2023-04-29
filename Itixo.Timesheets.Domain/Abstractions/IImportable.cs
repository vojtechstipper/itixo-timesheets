using System;

namespace Itixo.Timesheets.Domain.Abstractions;

public interface IImportable
{
    DateTimeOffset ImportedDate { get; set; }
}