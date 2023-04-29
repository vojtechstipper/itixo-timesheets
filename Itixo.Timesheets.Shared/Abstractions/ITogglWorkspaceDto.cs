using System;

namespace Itixo.Timesheets.Shared.Abstractions;

public interface ITogglWorkspaceDto
{
    int? Id { get; set; }
    string Name { get; set; }
    bool? Ispremium { get; set; }
    DateTime? UpdatedOn { get; set; }
}